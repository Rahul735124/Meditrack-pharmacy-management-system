import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowSuppliersComponent } from './show-suppliers.component';

describe('SupplierListComponent', () => {
  let component: ShowSuppliersComponent;
  let fixture: ComponentFixture<ShowSuppliersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShowSuppliersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowSuppliersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
