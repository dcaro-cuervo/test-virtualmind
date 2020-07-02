import { Component, OnInit, Input, Output, EventEmitter, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-modal',
    templateUrl: './modal.component.html',
    styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {
    headerText;
    bodyText;
    buttonText;
    
    constructor(private dialogRef: MatDialogRef<ModalComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

    ngOnInit(): void {
        this.headerText = this.data.headerText;
        this.bodyText = this.data.bodyText;
        this.buttonText = this.data.buttonText;
    }

    onClose(): void {
        this.dialogRef.close();
    }
}
