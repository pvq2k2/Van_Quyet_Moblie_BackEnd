// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $.validator.addMethod("regexEmail", function (value, element) {
        var emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        return this.optional(element) || emailRegex.test(value);
    }, "Địa chỉ email không hợp lệ !");

    $.validator.addMethod("regexPhoneNumber", function (value, element) {
        var phoneNumberRegex = /^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$/;
        return this.optional(element) || phoneNumberRegex.test(value);
    }, "Số điện thoại không hợp lệ !");

    $.validator.addMethod("regexPassword", function (value, element) {
        var passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,30}$/;
        return this.optional(element) || passwordRegex.test(value);
    }, "Mật khẩu phải chứa ít nhất 8 ký tự, bao gồm chữ cái viết hoa, viết thường, chữ số và ký tự đặc biệt !");

    $("#formAuth").validate({
        rules: {
            fullName: {
                required: true,
            },
            email: {
                required: true,
                regexEmail: true
            },
            //so_dien_thoai: {
            //    required: true,
            //    number: true,
            //    digits: true,
            //    regexPhoneNumber: true
            //},
            password: {
                required: true,
                regexPassword: true
            },
            confirmPassword: {
                required: true,
                regexPassword: true,
                equalTo: "#password",
            },
        },
        messages: {
            fullName: {
                required: "Vui lòng nhập họ và tên !",
            },
            email: {
                required: "Vui lòng nhập email !",
            },
            //so_dien_thoai: {
            //    required: "Vui lòng nhập số điện thoại !",
            //    number: "Vui lòng nhập số !",
            //    digits: "Vui lòng nhập số nguyên dương !"
            //},
            password: {
                required: "Vui lòng nhập mật khẩu !",
            },
            confirmPassword: {
                required: "Vui lòng nhập mật khẩu !",
                equalTo: "Mật khẩu không trùng khớp !",
            },
        },
        errorPlacement: function (error, element) {
            error.appendTo(element.closest(".form-group").find(".error-message"));
        },
        highlight: function (element) {
            $(element).closest(".form-group").find("#fullName").addClass("is-invalid");
            $(element).closest(".form-group").find("#email").addClass("is-invalid");
            $(element).closest(".form-group").find("#password").addClass("is-invalid");
            $(element).closest(".form-group").find("#confirmPassword").addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).closest(".form-group").find("#fullName").removeClass("is-invalid");
            $(element).closest(".form-group").find("#email").removeClass("is-invalid");
            $(element).closest(".form-group").find("#password").removeClass("is-invalid");
            $(element).closest(".form-group").find("#confirmPassword").removeClass("is-invalid");
        },
        submitHandler: function (form) {
            if (this.checkForm()) {
                form.submit();
            } else {
                $(form).find(":input.error:first").focus();
            }
            return false;
        }
    });
});
