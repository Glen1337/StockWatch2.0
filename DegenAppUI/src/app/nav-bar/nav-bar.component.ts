import { Component, OnInit } from '@angular/core';
import { AuthButtonComponent } from '../auth/login-button'

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  public NavItems: Array<{ label: string, link: string[] }>;
  title = 'ToTheMoon';

  constructor() {
    this.NavItems = [
      { label: 'Portfolios', link: ['/portfolios'] },
      { label: 'Research', link: ['/research'] },
      { label: 'Orders', link: ['/orders'] },
      { label: 'Company', link: ['/company'] },
      { label: 'Market', link: ['/market'] },
      { label: 'Watchlist', link: ['/watchlist'] },
      { label: 'Options', link: ['/options'] }
    ];
  }

  ngOnInit(): void { }

}
