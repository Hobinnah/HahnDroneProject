import { TestBed } from '@angular/core/testing';

import { DatahServiceService } from './datah-service.service';

describe('DatahServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DatahServiceService = TestBed.get(DatahServiceService);
    expect(service).toBeTruthy();
  });
});
