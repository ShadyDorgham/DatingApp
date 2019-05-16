import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemebrEditComponent } from '../members/memebr-edit/memebr-edit.component';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<MemebrEditComponent> {
    canDeactivate(component: MemebrEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
        }
        return true;
    }
}
