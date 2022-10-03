/*=========================================================================================
  File Name: auth-register.js
  Description: Auth register js file.
  ----------------------------------------------------------------------------------------
  Item Name: Vuexy  - Vuejs, HTML & Laravel Admin Dashboard Template
  Author: PIXINVENT
  Author URL: http://www.themeforest.net/user/pixinvent
==========================================================================================*/

$(function () {
    ('use strict');

    var assetsPath = '../../../app-assets/',
        registerMultiStepsWizard = document.querySelector('.register-multi-steps-wizard'),
        pageResetForm = $('.auth-register-form'),
        select = $('.select2'),
        creditCard = $('.credit-card-mask'),
        expiryDateMask = $('.expiry-date-mask'),
        cvvMask = $('.cvv-code-mask'),
        mobileNumberMask = $('.mobile-number-mask'),
        pinCodeMask = $('.pin-code-mask');

    if ($('body').attr('data-framework') === 'laravel') {
        assetsPath = $('body').attr('data-asset-path');
    }

    // jQuery Validation
    // --------------------------------------------------------------------
    if (pageResetForm.length) {
        pageResetForm.validate({
            /*
            * ? To enable validation onkeyup
            onkeyup: function (element) {
              $(element).valid();
            },*/
            /*
            * ? To enable validation on focusout
            onfocusout: function (element) {
              $(element).valid();
            }, */
            rules: {
                'register-username': {
                    required: true
                },
                'register-email': {
                    required: true,
                    email: true
                },
                'register-password': {
                    required: true
                },
                'merchant-password': {
                    required: true
                },
                'merchant-password-check': {
                    required: true,
                    equalTo: '#merchant-password'
                },
            }
        });
    }

    // multi-steps registration
    // --------------------------------------------------------------------

    // Horizontal Wizard
    if (typeof registerMultiStepsWizard !== undefined && registerMultiStepsWizard !== null) {
        var numberedStepper = new Stepper(registerMultiStepsWizard),
            $form = $(registerMultiStepsWizard).find('form');
        $form.each(function () {
            var $this = $(this);
            $this.validate({
                rules: {
                    username: {
                        required: true
                    },
                    email: {
                        required: true
                    },
                    password: {
                        required: true,
                        minlength: 8
                    },
                    'confirm-password': {
                        required: true,
                        minlength: 8,
                        equalTo: '#password'
                    },
                    'FirstName': {
                        required: true,
                        minlength: 3,
                        maxlength: 50
                    },
                    'LastName': {
                        required: true,
                        minlength: 3,
                        maxlength: 50
                    },
                    'Email': {
                        required: true,
                        minlength: 3,
                    },
                    'Phone': {
                        required: true,
                        minlength: 12,
                        maxlength: 12
                    },
                    'TaxId': {
                        required: true,
                        minlength: 10,
                        maxlength: 11
                    },
                    'TaxHouse': {
                        required: true,
                        minlength: 3,
                    },
                    'Address': {
                        required: true,
                        minlength: 3,
                        maxlength: 250,
                    },
                    'CompanyName': {
                        required: true,
                        minlength: 3
                    },
                    'EmailAddress': {
                        required: true,
                        minlength: 3
                    },
                    'merchant-password': {
                        required: true
                    },
                    'merchant-password-check': {
                        required: true,
                        equalTo: '#merchant-password'
                    },
                    'home-address': {
                        required: true
                    },
                    addCard: {
                        required: true
                    }
                },
                messages: {
                    password: {
                        required: 'Enter new password',
                        minlength: 'Enter at least 8 characters'
                    },
                    'confirm-password': {
                        required: 'Please confirm new password',
                        minlength: 'Enter at least 8 characters',
                        equalTo: 'The password and its confirm are not the same'
                    }
                }
            });
        });
        $(registerMultiStepsWizard)
            .find('.btn-next')
            .each(function () {
                $(this).on('click', function (e) {
                    var isValid = $(this).parent().siblings('form').valid();
                    if (isValid && $('#CompanyType').val() != 0 && $('#CityId').val() != 0 && $('#DistrictId').val() != 0) {
                        numberedStepper.next();
                    } else {
                        if ($('#CompanyType').val() == 0) {
                            $('#CompanyType-Required').show().html("Bu alan boş bırakılamaz")
                            $('#CompanyType').addClass("is-invalid error")
                        }
                        if ($('#CityId').val() == 0) {
                            $('#CityId-Required').show().html('İl seçin')
                            $('#CityId').addClass("is-invalid error")
                        }
                        if ($('#DistrictId').val() == 0) {
                            $('#DistrictId-Required').show().html('İlçe seçin')
                            $('#DistrictId').addClass("is-invalid error")
                        }
                        e.preventDefault();
                    }
                });
            });

        $(registerMultiStepsWizard)
            .find('.btn-prev')
            .on('click', function () {
                numberedStepper.previous();
            });

        $(registerMultiStepsWizard)
            .find('.btn-submit')
            .on('click', function () {
                var isValid = $(this).parent().siblings('form').valid();
                if (isValid) {
                    alert('Submitted..!!');
                }
            });
    }
    $('#CompanyType').change(function () {
        if ($(this).val() != 0) {
            $('#CompanyType-Required').hide().html("")
            $('#CompanyType').removeClass("is-invalid error")
        }
    })
    $('#CityId').change(function () {
        if ($(this).val() != 0) {
            $('#CityId-Required').hide().html("")
            $('#CityId').removeClass("is-invalid error")
        }
    })
    $('#DistrictId').change(function () {
        if ($(this).val() != 0) {
            $('#DistrictId-Required').hide().html("")
            $('#DistrictId').removeClass("is-invalid error")
        }
    })
    $('#AydinlatmaMeni').change(function () {
        if ($(this).is(":checked") == true) {
            $('#AydinlatmaMeni-Required').hide().html("")
        }
    })
    // select2
    select.each(function () {
        var $this = $(this);
        $this.wrap('<div class="position-relative"></div>');
        $this.select2({
            // the following code is used to disable x-scrollbar when click in select input and
            // take 100% width in responsive also
            dropdownAutoWidth: true,
            width: '100%',
            dropdownParent: $this.parent()
        });
    });

    // credit card

    // Credit Card
    if (creditCard.length) {
        creditCard.each(function () {
            new Cleave($(this), {
                creditCard: true,
                onCreditCardTypeChanged: function (type) {
                    const elementNodeList = document.querySelectorAll('.card-type');
                    if (type != '' && type != 'unknown') {
                        //! we accept this approach for multiple credit card masking
                        for (let i = 0; i < elementNodeList.length; i++) {
                            elementNodeList[i].innerHTML =
                                '<img src="' + assetsPath + 'images/icons/payments/' + type + '-cc.png" height="24"/>';
                        }
                    } else {
                        for (let i = 0; i < elementNodeList.length; i++) {
                            elementNodeList[i].innerHTML = '';
                        }
                    }
                }
            });
        });
    }

    // Expiry Date Mask
    if (expiryDateMask.length) {
        new Cleave(expiryDateMask, {
            date: true,
            delimiter: '/',
            datePattern: ['m', 'y']
        });
    }

    // CVV
    if (cvvMask.length) {
        new Cleave(cvvMask, {
            numeral: true,
            numeralPositiveOnly: true
        });
    }

    // phone number mask
    if (mobileNumberMask.length) {
        new Cleave(mobileNumberMask, {
            phone: true,
            phoneRegionCode: 'US'
        });
    }

    // Pincode
    if (pinCodeMask.length) {
        new Cleave(pinCodeMask, {
            delimiter: '',
            numeral: true
        });
    }

    // multi-steps registration
    // --------------------------------------------------------------------
});
