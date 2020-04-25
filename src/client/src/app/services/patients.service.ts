import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, throwError, of, ReplaySubject } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { PatientData } from '../data/patient';
import { PatientsCollection } from './patientsCollection';

@Injectable({
  providedIn: 'root'
})
export class PatientsService {

  public httpErrorObs: ReplaySubject<HttpErrorResponse> = new ReplaySubject<HttpErrorResponse>(1);

  constructor(private http: HttpClient) {
  }

  getAllPatients(currentPage: number, resultSize: number ){

    const options = {params: new HttpParams().set('index', currentPage.toString()).set('size', resultSize.toString())};


    return this.http.get<PatientsCollection>(environment.serverUrl, options)
      .pipe(
        retry(3),
        catchError(this.handleGetError.bind(this))
      );
  }

  addPatient(name: string) {

    const patientData = { "patientName" : name};

    return this.http.post(environment.serverUrl, patientData)
    .pipe(
      retry(1),
      catchError(this.handlePostError.bind(this))
    )
  }

  patientHandled(patient: PatientData) {

    return this.http.put(environment.serverUrl, patient)
                    .pipe(
                      retry(1),
                      catchError(this.handlePutError.bind(this))
                    );

  }

  private handleGetError(error: HttpErrorResponse) : Observable<PatientsCollection> {

    this.httpErrorObs.next(error);

    return of({ "data" : [], "totalCount": 0, "pagesCount": 0});  // Empty array
  }

  private handlePostError(error: HttpErrorResponse) : Observable<PatientData> {
    this.httpErrorObs.next(error);

    return of ({"ticketNumber": -1, "name": "", "time": ""});
  }

  private handlePutError(error: HttpErrorResponse) {
    this.httpErrorObs.next(error);
  }
}
