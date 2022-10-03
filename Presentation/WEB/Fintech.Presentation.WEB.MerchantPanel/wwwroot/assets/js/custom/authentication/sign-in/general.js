//"use strict";
//var KTSigninGeneral = function () {
//    var t, e, i;
//    return {
//        init: function () {
//            t = document.querySelector("#kt_sign_in_form"),
//                e = document.querySelector("#kt_sign_in_submit"),
//                i = FormValidation.formValidation(t, {
//                    fields: {
//                        email: {
//                            validators: {
//                                notEmpty: {
//                                    message: "Email address is required"
//                                },
//                                emailAddress: {
//                                    message: "The value is not a valid email address"
//                                }
//                            }
//                        },
//                        password: {
//                            validators: {
//                                notEmpty: {
//                                    message: "The password is required"
//                                }
//                            }
//                        }
//                    },
//                    plugins: {

//                        trigger: new FormValidation.plugins.Trigger,
//                        bootstrap: new FormValidation.plugins.Bootstrap5({
//                            rowSelector: ".fv-row"
//                        }),
//                        submitButton: new FormValidation.plugins.SubmitButton({


//                        })
//                    }
//                }), e.addEventListener("click", (function (n) {



//                    n.preventDefault(),
//                        i.validate().then((function (i) {
//                            PostLogin();
//                        }))
//                }))
//        }
//    }
//}();
//KTUtil.onDOMContentLoaded((function () {
//    KTSigninGeneral.init()
//}));
//$("#kt_sign_in_submit").flatpickr({
//    alert();
//});
async function PostLogin() {
  var formData = document.querySelector("#kt_sign_in_form");
  await  $.ajax({
        type: "POST",
        url: "/Auth/Login",
        data: formData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.Success != false) {

                window.location.href = "/home/index";
            }

        },
        failure: function (response) {
                window.location.href = "/home/index";
          
        },
        error: function (response) {
                window.location.href = "/home/index";
         
        }
    });


}





