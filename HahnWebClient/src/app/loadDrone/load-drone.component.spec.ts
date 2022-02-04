import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadDroneComponent } from './load-drone.component';

describe('LoadDroneComponent', () => {
  let component: LoadDroneComponent;
  let fixture: ComponentFixture<LoadDroneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoadDroneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoadDroneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
