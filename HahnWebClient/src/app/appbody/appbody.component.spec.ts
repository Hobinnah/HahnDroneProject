import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppbodyComponent } from './appbody.component';

describe('AppbodyComponent', () => {
  let component: AppbodyComponent;
  let fixture: ComponentFixture<AppbodyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppbodyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppbodyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
