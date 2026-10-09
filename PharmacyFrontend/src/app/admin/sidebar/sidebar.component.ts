import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
//   styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent {
  @Output() viewChange = new EventEmitter<string>();

  changeView(view: string): void {
    this.viewChange.emit(view);
  }
}
