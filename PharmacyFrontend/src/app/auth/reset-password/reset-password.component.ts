import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule, RouterModule]
})
export class ResetPasswordComponent implements OnInit {
    resetPasswordForm: FormGroup;
    token: string = '';
    email: string = '';
    message: string = '';

    constructor(private route: ActivatedRoute, private fb: FormBuilder, private authService: AuthService, private router: Router) {
        this.resetPasswordForm = this.fb.group({
            newPassword: ['', [Validators.required, Validators.minLength(6)]],
            confirmPassword: ['', Validators.required]
        });
    }

    ngOnInit() {
        // Extract token and email from URL
        this.route.queryParams.subscribe(params => {
            this.token = decodeURIComponent(params['token']);
            this.email = params['email'];
        });
    }

    resetPassword() {
        if (this.resetPasswordForm.invalid || this.resetPasswordForm.value.newPassword !== this.resetPasswordForm.value.confirmPassword) {
            this.message = 'Passwords do not match.';
            return;
        }

        this.authService.resetPassword(this.email, this.token, this.resetPasswordForm.value.newPassword, this.resetPasswordForm.value.confirmPassword).subscribe({
            next: () => {
                console.log("Token:", this.token);
                console.log("Email:", this.email);
                console.log("New Password:", this.resetPasswordForm.value.newPassword);
                this.message = 'Password reset successful.';
                setTimeout(() => this.router.navigate(['/auth/login']), 2000);
            },
            error: () => this.message = 'Failed to reset password.'
        });
    }
}