$(document).ready(function () {
    $('#ChooseImg').change(function (e) {
        var url = $('#ChooseImg').val();
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        if (ChooseImg.files && ChooseImg.files[0] && (ext == "gif" || ext == "png" || ext == "jpg" || ext == "jpeg")) 
        {
            var reader = new FileReader();
            reader.onload = function () {
                var output = document.getElementById('PrevImg');
                output.src = reader.result;
            }
            reader.readAsDataURL(e.target.files[0]);
        }
    });
});