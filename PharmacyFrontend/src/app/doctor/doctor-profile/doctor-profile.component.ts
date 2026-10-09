import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { DoctorService } from '../../services/doctor.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

@Component({
    selector: 'app-doctor-profile',
    standalone: true,
    imports: [FormsModule, ReactiveFormsModule, CommonModule, RouterModule],
    templateUrl: './doctor-profile.component.html'
})
export class DoctorProfileComponent implements OnInit {
    profileForm!: FormGroup;
    doctorProfile: any = {};
    successMessage = '';
    errorMessage = '';
    isEditing = false; // Toggle form visibility

    constructor(private fb: FormBuilder, private doctorService: DoctorService,  private router:Router) { }

    ngOnInit(): void {
        this.profileForm = this.fb.group({
            fullName: ['', Validators.required],
            contact: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
        });

        this.loadProfile();
    }

    loadProfile() {
        this.doctorService.getProfile().subscribe({
            next: (res) => {
                this.doctorProfile = res.data;
                this.profileForm.patchValue(res.data);
            },
            error: () => {
                this.errorMessage = 'Failed to load profile';
            },
        });
    }

    toggleEdit() {
        this.isEditing = !this.isEditing;
    }

    onSubmit() {
        if (this.profileForm.invalid) return;

        this.doctorService.updateProfile(this.profileForm.value).subscribe({
            next: () => {
                this.successMessage = 'Profile updated successfully';
                this.errorMessage = '';
                this.doctorProfile = this.profileForm.value;
                this.isEditing = false; // Hide form after update
            },
            error: () => {
                this.successMessage = '';
                this.errorMessage = 'Failed to update profile';
            },
        });
    }

    navigateToDashboard() {
        this.router.navigate(['/dashboard/doctor']); // Redirect to dashboard route
    }

}