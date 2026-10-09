import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowDrugsComponent } from './show-drugs.component';

describe('DrugListComponent', () => {
  let component: ShowDrugsComponent;
  let fixture: ComponentFixture<ShowDrugsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShowDrugsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowDrugsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
