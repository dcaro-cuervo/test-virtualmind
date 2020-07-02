import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, FormGroupDirective, NgForm } from '@angular/forms';
import { FormBase } from '../../helper/form-base';
import { CrossFieldErrorMatcher } from '../../helper/cross-field-error-matcher';
import { CurrencyPipe } from '../../helper/currency.pipe';
import { PurchaseService } from '../../shared/purchase.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalComponent } from '../../components/modal/modal.component';

@Component({
    selector: 'app-purchase',
    templateUrl: './purchase.component.html',
    styleUrls: ['./purchase.component.scss'],
    providers: [CurrencyPipe]
})
export class PurchaseComponent extends FormBase implements OnInit, AfterViewInit {
    // Set the component where we will start to validations.
    @ViewChild('userId') firstItem: ElementRef;

    // Declarate directive to reset validators.
    @ViewChild(FormGroupDirective) formDirective: FormGroupDirective;

    purchaseFormGroup: FormGroup;

    errorMatcher = new CrossFieldErrorMatcher();

    currencyArray: any[] = [
        { value: 'Dolar', viewValue: 'Dolar' },
        { value: 'Real', viewValue: 'Real' }
    ];

    constructor(private fb: FormBuilder, private purchaseService: PurchaseService, private dialog: MatDialog) {
        super();

        // For each control with validation rules, add an object with the required validation messages.
        this.validationMessages = {
            userId: {
                required: 'User name is required.',
                minlength: 'User name minimum length is 6.',
                maxlength: 'User name maximum length is 15.',
                pattern: 'User name minimum length 6, allowed characters letters, numbers only. No spaces.'
            },
            amount: {
                required: 'Amount is required.',
                maxlength: 'Amount name maximum length is 6.'
            },
            currency: {
                required: 'Currency is required.',
            }
        };

        // For each control with validation rules, add the control name to this object.
        this.formErrors = {
            userId: '',
            amount: '',
            currency: ''
        };
    }

    ngOnInit(): void {
        // Create reactive form.
        this.createGroup();
    }

    ngAfterViewInit(): void {
        // Monitors each control value change, control blur event, and then invokes a method to check the controls and set their validation error messages.
        setTimeout(() => {
            this.firstItem.nativeElement.focus();
        }, 250);

        this.startControlMonitoring(this.purchaseFormGroup);
    }

    createGroup() {
        const config = {
            userId: ['', [
                Validators.required,
                Validators.minLength(6),
                Validators.maxLength(15),
                Validators.pattern('^[a-zA-Z0-9]*$')]],
            amount: ['', [
                Validators.required,
                Validators.maxLength(6)]],
            currency: ['', [Validators.required]],

        }

        this.purchaseFormGroup = this.fb.group(config);
    }

    purchase() {
        if (this.purchaseFormGroup.invalid) {
            return;
        }

        this.purchaseService.postPurchase(this.purchaseFormGroup.value).subscribe(x => {
            this.openDialog();
        });
    }

    openDialog() {
        this.dialog.open(ModalComponent, {
            autoFocus: true,
            data: {
                headerText: 'Success Purchase',
                bodyText: 'Your purchase has been successfully completed.',
                buttonText: 'OK'
            },
        }).afterClosed().subscribe(result => {
            this.clearForm();
        });
    }

    clearForm() {
        // Clear controls.
        this.purchaseFormGroup.reset();

        // Clear validators.
        this.formDirective.resetForm();

        // Focus on userId control.
        this.firstItem.nativeElement.focus();
    }
}
