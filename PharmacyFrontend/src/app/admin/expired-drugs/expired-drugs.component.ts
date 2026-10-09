import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../services/admin.service';
import { HttpClientModule } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
    selector: 'app-expired-drugs',
    standalone: true,
    imports: [CommonModule, HttpClientModule],
    templateUrl: './expired-drugs.component.html'
})
export class ExpiredDrugsComponent implements OnInit {
    expiredDrugs: any[] = [];
    loading = true;

    constructor(private adminService: AdminService) { }

    ngOnInit(): void {
        this.adminService.getExpiredDrugs().subscribe({
            next: res => {
                this.expiredDrugs = res.data;
                this.loading = false;
            },
            error: err => {
                console.error('Error fetching expired drugs:', err);
                this.loading = false;
            }
        });
    }

    onDeleteDrug(id: number) {
        if (confirm('Are you sure you want to delete this drug?')) {
            this.adminService.deleteDrug(id.toString()).subscribe({
                next: () => {
                    this.expiredDrugs = this.expiredDrugs.filter(drug => drug.id !== id);
                    alert('Drug deleted successfully.');
                },
                error: (err) => {
                    console.error('Delete error:', err);
                    alert('Failed to delete the drug.');
                }
            });
        }
    }

    onRemoveAll() {
        if (confirm('Are you sure you want to remove all expired drugs?')) {
            const deleteRequests = this.expiredDrugs.map(drug =>
                this.adminService.deleteDrug(drug.id.toString())
            );

            forkJoin(deleteRequests).subscribe({
                next: () => {
                    this.expiredDrugs = [];
                    alert('All expired drugs have been removed.');
                },
                error: (err) => {
                    console.error('Bulk delete error:', err);
                    alert('Failed to remove all expired drugs.');
                }
            });
        }
    }


}
