import { TestBed } from '@angular/core/testing';

import { InventoryorderService } from './inventoryorder.service';

describe('InventoryorderService', () => {
  let service: InventoryorderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InventoryorderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
