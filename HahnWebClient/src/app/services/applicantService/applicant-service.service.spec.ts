import { TestBed } from '@angular/core/testing';

import { ApplicantServiceService } from './applicant-service.service';

describe('ApplicantServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ApplicantServiceService = TestBed.get(ApplicantServiceService);
    expect(service).toBeTruthy();
  });
});
