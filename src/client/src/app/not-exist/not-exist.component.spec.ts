import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotExistComponent } from './not-exist.component';

describe('NotExistComponent', () => {
  let component: NotExistComponent;
  let fixture: ComponentFixture<NotExistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotExistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotExistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
