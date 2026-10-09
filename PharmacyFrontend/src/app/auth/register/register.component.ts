import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  standalone: true,
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  registerForm: FormGroup;
  confirmPassword: string = '';
  errorMessage: string = '';
  successMessage: string = '';
  passwordMismatch: boolean = false; // ✅ Add this line
  showPassword: boolean = false;
  showConfirmPassword: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });
  }

  togglePassword(field: string): void {
    if (field === 'password') {
      this.showPassword = !this.showPassword;
    } else if (field === 'confirmPassword') {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }


  register():void {
    this.errorMessage = '';
    this.successMessage = '';
    this.passwordMismatch = false;

    if (this.registerForm.invalid) {
      this.errorMessage = 'Please fill out all fields correctly.';
      return;
    }

    const { password, confirmPassword, ...rest } = this.registerForm.value;
    if (password !== confirmPassword) {
      this.passwordMismatch = true;
      this.errorMessage = 'Passwords do not match.';
      return;
    }
    this.authService.register({...rest, password}).subscribe({
      next: () =>{
        this.successMessage="Registered successfully!";
        this.registerForm.reset();
      },
      error: err => this.errorMessage=err.error?.message || "Registration failed."
    });

    const { name, email, contact } = this.registerForm.value;
    const userData = { name, email, contact, password };

    console.log('Registering user:', userData);
    this.successMessage = 'Registration successful!';
    this.registerForm.reset();
  }
}
