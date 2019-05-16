/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MemebrEditComponent } from './memebr-edit.component';

describe('MemebrEditComponent', () => {
  let component: MemebrEditComponent;
  let fixture: ComponentFixture<MemebrEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MemebrEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MemebrEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
