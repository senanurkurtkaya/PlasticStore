document.addEventListener("DOMContentLoaded", function () {
    const mainImage = document.getElementById("mainImage");
    const thumbnails = document.querySelectorAll(".thumbnail-image");

    if (mainImage && thumbnails.length > 0) {
        thumbnails.forEach(thumbnail => {
            //thumbnail.addEventListener("click", function () {
            //    const newSrc = this.getAttribute("data-src");
            //    mainImage.setAttribute("src", newSrc);
            //});
            
            thumbnail.addEventListener("mouseenter", function () {
                const newSrc = this.getAttribute("data-src");
                mainImage.style = `background-image: url('${newSrc}')`
                //mainImage.setAttribute("src", newSrc);
            });
        });
    }
});