import { Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Portfolio } from '../Models/Portfolio';
import { PortfolioDataService } from '../Services/portfolio-data.service';
import { PortfolioTypes } from '../Models/Constants';

@Component({
  selector: 'portfolio-list',
  templateUrl: './portfolio-list.component.html',
  styleUrls: ['./portfolio-list.component.css']
})
export class PortfolioListComponent implements OnInit, OnDestroy {

  private subscriptions: Subscription[] = []
  public portfolios: Array<Portfolio> = [];
  public errorMsg: string = "";

  public portfolioForm = new FormGroup({
    portfolioTitleControl: new FormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(20)
    ]),
    portfolioTypeControl: new FormControl('',[
      Validators.min(1.00),
      Validators.maxLength(30),
      Validators.required
    ])
  });

  portfolioTypes = [ 
    PortfolioTypes.DayTrading,
    PortfolioTypes.Investment,
    PortfolioTypes.Retirement,
    PortfolioTypes.SwingTrading,
    PortfolioTypes.Other,
    PortfolioTypes.Speculation,
  ];
  
  constructor(private route: ActivatedRoute, private location: Location, private portfolioDataService: PortfolioDataService) {}

  get portfolioTitleControl() { return this.portfolioForm.get('portfolioTitleControl'); }
  
  get portfolioTypeControl() { return this.portfolioForm.get('portfolioTypeControl'); }

  ngOnInit(): void {
    let sub: Subscription = new Subscription();

    sub = this.route.data.subscribe(
      (data) => {
        this.portfolios = data.portfolios;
        if (data.portfolios.length === 0){
          //this.errorMsg = "Could not retrieve portfolios from server"
        }
      },
      (error) => {
        console.log(`(component)Error getting portfolio list:${error}`);
        this.errorMsg = error.error;
      },
      () => { console.log("Completed retrieval of portfolio list");}
    );

    this.subscriptions.push(sub)
  }

  deletePortfolio(event: any, portfolioId: number){
    let subscription3: Subscription = new Subscription();
    subscription3 = this.portfolioDataService.deletePortfolio(portfolioId).subscribe(
      (response) => {
        this.portfolios.forEach((port, index) => {
          if(port.portfolioId === portfolioId) this.portfolios.splice(index,1);
        });
        console.log(`Portfolio deleted: ${portfolioId}`);
      },
      (error) => { 
        this.errorMsg = `${error.error}`
        console.log('(component)Error in deletePortfolio ', error);
      },
      () => { console.log(`(component)Delete Portfolio complete`); }
    );
    
    this.subscriptions.push(subscription3)
  }

  onSubmitPortfolio(): void {
    let subscription2: Subscription = new Subscription();

    let port: Portfolio = {
      title: String(this.portfolioTitleControl?.value).trim(),
      portfolioId: 0,
      totalMarketValue: 0,
      gainLoss: 0,
      orders: [],
      holdings: [],
      type: String(this.portfolioTypeControl?.value).trim()
    }

    console.log("(component)Sending portfolio post to API: ", port);

    subscription2 = this.portfolioDataService.addPortfolio(port)
      .subscribe(
        (returnedPort) => { this.portfolios.push(returnedPort); },
        (error) => {
          console.log('(component)Error in onSubmitPortfolio: ', error);
          this.errorMsg = `${error.error}`;
        },
        () => { console.log(`addportfolio completed`); }
      );
    this.subscriptions.push(subscription2)
  }

  messageClick() {
    this.errorMsg = '';
 }
    
  ngOnDestroy() {
    if (this.subscriptions && this.subscriptions.length > 0) {
      this.subscriptions.forEach((sub) => {
        if (!sub.closed) { sub.unsubscribe(); }     
      });
    }
  }

  GetOptionCount(id: number){
    return this.portfolios.find(port => port.portfolioId == id)?.holdings?.filter(h => h.securityType === "Call" || h.securityType === "Put").length
  }

  GetEquityCount(id: number){
    return this.portfolios.find(port => port.portfolioId == id)?.holdings?.filter(h => h.securityType === "Share").length
  }

  ConvertDate(date?: Date){
    return(date ? new Date(date).toLocaleString() : '');
  }

  goBack(): void {
    this.location.back();
  }

  refresh(): void {
    location.reload();
  }

}
