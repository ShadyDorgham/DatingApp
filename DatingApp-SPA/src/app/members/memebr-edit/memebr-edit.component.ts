import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { error } from '@angular/compiler/src/util';

@Component({
  selector: 'app-memebr-edit',
  templateUrl: './memebr-edit.component.html',
  styleUrls: ['./memebr-edit.component.css']
})
export class MemebrEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
user: User;
photoUrl: string;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }


  constructor(private route: ActivatedRoute, private alertify: AlertifyService,
     private userService: UserService , private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);

  }



updateUser() {
  this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
    this.alertify.success('Profile updated successfully');
    this.editForm.reset(this.user);
  // tslint:disable-next-line:no-shadowed-variable
  }, error => {
    this.alertify.error(error);
  });
}

updateMainPhoto(photoUrl) {
 this.user.photoUrl = photoUrl;
}


}

