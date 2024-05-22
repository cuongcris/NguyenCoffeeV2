$(document).ready(function () {
    $('#btnSubmit').click(function () {
        console.log("Button clicked");
        $.ajax({
            url: "/AiHandle/Index?handler=LoadImages",
            type: "GET",
            dataType: "json",
            success: function (datas) {
                const imageContainer = document.getElementById('imageContainer');
                imageContainer.innerHTML = ''; // Clear previous images

                datas.forEach(function (image) {
                    const imageElement = document.createElement('div');
                    imageElement.classList.add('text-center', 'col-md-6', 'image-container');
                    imageElement.innerHTML = `<img class="imagAi" src="${image.Image}" alt="Image">`;
                    imageContainer.appendChild(imageElement);
                    console.log(imageElement);
                });
            },
            error: function (xhr, status, error) {
                console.error("JQuery AJAX error: ", error);
            }
        });
    });
});