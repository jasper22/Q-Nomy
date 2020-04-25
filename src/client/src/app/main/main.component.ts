import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormControl, Validators, FormGroup, ValidatorFn, AbstractControl } from '@angular/forms';
import { ClrLoadingState } from '@clr/angular';
import { PatientData } from '../data/patient';
import { PatientsService } from '../services/patients.service';
import {
  Router,
  Event as RouterEvent,
  NavigationStart,
  NavigationEnd,
  NavigationCancel,
  NavigationError
} from '@angular/router'
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit, AfterViewInit {

  addPatientButtonState:ClrLoadingState = ClrLoadingState.DEFAULT;
  callNextPatientButtonState:ClrLoadingState = ClrLoadingState.DEFAULT;

  public showOverlay = true;

  public showError = false;

  public errorDescription: string = '';

  private subscription:Subscription;

  patientName = new FormControl('', [Validators.required, Validators.minLength(3), this.checkBasicNameValidity()]);

  currentPatient: PatientData = null;

  allPatients: Array<PatientData> = new Array<PatientData>();

  totalNumberOfRecords: number;

  currentPage: number = 1;

  pageSize: number;

  constructor(private patientService: PatientsService, private router:Router) {
    router.events.subscribe((event: RouterEvent) => {
      this.navigationInterceptor(event)
    });

    this.patientService.httpErrorObs.subscribe(err => {
      this.handleError(err);
    });
  }

  ngAfterViewInit(): void {

  }

  ngOnInit(): void {

    this.pageSize = 10;

    this.showOverlay = true;

    this.retryLoad();
  }

  retryLoad() {
    this.showError = false;

    if (this.subscription != null) {
      this.subscription.unsubscribe();
    }

    this.subscription = this.patientService.getAllPatients(this.currentPage, this.pageSize).subscribe(patientsCollecton => {
      this.allPatients = patientsCollecton.data;
      this.totalNumberOfRecords = patientsCollecton.totalCount;
      this.showOverlay = false;
    });
  }

  checkBasicNameValidity(): ValidatorFn {
    return (control: AbstractControl) : { [key:string]: any} | null => {
      const patientName = control.value?.toString();
      const parts = patientName?.split(" ");

      if (parts?.length <= 1) {
        return {'incorrectName': {value: control.value}};
      }

      parts?.forEach(element => {
        if ((element == null) || (element == undefined) || (element.length == 0)) {
          return {'incorrectName': {value: control.value}};
        }
      });

      // Valid
      return null;
    };
  }

  navigationInterceptor(event: RouterEvent): void {
    if (event instanceof NavigationStart) {
      this.showOverlay = true;
    }

    // Set loading state to false in both of the below events to hide the spinner in case a request fails
    if (event instanceof NavigationCancel) {
      this.showOverlay = false;
    }
    if (event instanceof NavigationError) {
      this.showOverlay = false;
    }
  }

  addPatientName() {
    this.addPatientButtonState = ClrLoadingState.LOADING;

    const patient = new PatientData().name = this.patientName.value;

    this.patientService.addPatient(patient).subscribe(addedPatient => {

      this.allPatients.push(addedPatient);

      this.addPatientButtonState = ClrLoadingState.SUCCESS;

      this.patientName.reset();
    });
  }

  callNextPatient() {
    this.callNextPatientButtonState = ClrLoadingState.LOADING;

    this.currentPatient = this.allPatients[0];

    // First we update server side
    this.patientService.patientHandled(this.currentPatient).subscribe(result => {

      this.allPatients.splice(0, 1);

      this.callNextPatientButtonState = ClrLoadingState.DEFAULT;
    });
  }

  private handleError(error: HttpErrorResponse) {
    this.errorDescription = error.message;
    this.showOverlay = false;
    this.showError = true;
  }

  refreshDataGrid(event) {
    this.retryLoad();
  }

  pageSizeChange(event) {
    this.retryLoad();
  }
}
