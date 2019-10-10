import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingScreenService {

  private _loading: boolean;
  public get loading(): boolean { return this._loading; }
  public set loading(value) { this._loading = value; }

  constructor() {
    this.loading = false;
  }

  start() {
    this.loading = true;
  }

  stop() {
    this.loading = false;
  }
}
