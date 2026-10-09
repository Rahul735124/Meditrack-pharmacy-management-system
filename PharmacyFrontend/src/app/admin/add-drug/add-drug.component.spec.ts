import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDrugComponent } from './add-drug.component';

describe('AddDrugComponent', () => {
  let component: AddDrugComponent;
  let fixture: ComponentFixture<AddDrugComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddDrugComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDrugComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
