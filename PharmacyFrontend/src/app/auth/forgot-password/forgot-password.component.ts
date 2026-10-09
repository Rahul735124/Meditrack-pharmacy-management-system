import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html',
    standalone:true,
    imports:[RouterModule, ReactiveFormsModule, CommonModule]
})
export class ForgotPasswordComponent {
    forgotPasswordForm: FormGroup;
    message: string = '';

    constructor(private fb: FormBuilder, private authService: AuthService) {
        this.forgotPasswordForm = this.fb.group({
            email: ['', [Validators.required, Validators.email]]
        });
    }

    sendResetLink() {
        if (this.forgotPasswordForm.invalid) {
            this.message = 'Enter a valid email.';
            return;
        }

        this.authService.forgotPassword(this.forgotPasswordForm.value.email).subscribe({
            next: res => this.message = res.message,
            error: () => this.message = 'Failed to send reset link.'
        });
    }
}