import { TestBed } from '@angular/core/testing';

import { DispatchServiceService } from './dispatch-service.service';

describe('DispatchServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DispatchServiceService = TestBed.get(DispatchServiceService);
    expect(service).toBeTruthy();
  });
});
