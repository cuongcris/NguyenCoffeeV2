document.getElementById('btnSubmit').addEventListener('click', function () {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "AiHandle/Index?handler=LoadImages");
    xhr.onload = function () {
        if (xhr.status >= 200 && xhr.status < 300) {
            var datas = JSON.parse(xhr.responseText);
            const imageContainer = document.getElementById('imageContainer');

            datas.forEach(function (image) {
                const imageElement = document.createElement('div');
                imageElement.classList.add('text-center', 'col-md-6', 'image-container');
                imageElement.innerHTML = `<img class="imagAi" src="${image.Image}" alt="Image">`;
                imageContainer.appendChild(imageElement);
                //imageContainer.insertAdjacentHTML('beforeend', imageElement);
                console.log(imageElement);
            });

        }
        else {
            console.error(xhr.statusText);
        }
        xhr.send();
    };
});