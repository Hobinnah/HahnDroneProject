import { TestBed } from '@angular/core/testing';

import { DroneServiceService } from './drone-service.service';

describe('DroneServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DroneServiceService = TestBed.get(DroneServiceService);
    expect(service).toBeTruthy();
  });
});
