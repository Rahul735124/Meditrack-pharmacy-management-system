import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../auth.service';
import { CookieService } from 'ngx-cookie-service';

// import  

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule, CommonModule],
})
export class LoginComponent {

  loginForm: FormGroup;
  errorMessage: string = '';
  showPassword=false;
  password='';

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService, private cookieService: CookieService) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }
  togglePassword(){
    this.showPassword=!this.showPassword;
  }



  login() {
    if (this.loginForm.invalid) {
      this.errorMessage = 'Please fill in all fields correctly.';
      return;
    }
    this.authService.login(this.loginForm.value).subscribe({
      next: (response) => {
        console.log(response);
        // localStorage.setItem('token', response.data.token);
        this.cookieService.set('token', response.data.token, 1); // expires in 1 day
        // this.cookieService.set('user_role', response.data.role, 1);
        const role = response.data.role.toLowerCase();
        this.router.navigate([`/dashboard/${role.toLowerCase()}`]);
    // this.router.navigate([`/${role}/dashboard`]);
        
      },
        
        error: err => alert('Login failed. Please check your credentials.')


    });


    // TODO: Integrate actual login logic
    console.log(this.loginForm.value);

  }
}
