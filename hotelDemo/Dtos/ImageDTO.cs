using System.ComponentModel.DataAnnotations;

namespace hotelDemo.Dtos
{
    public class ImageDTO
    {
        public ImageDTO(string imageName, string folderName)
        {
            ImageName = imageName;
            FolderName = folderName;
        }

        [Required(ErrorMessage = "This field is required")]
        [MinLength(5, ErrorMessage = "A minimum of 5 characters is required")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MinLength(5, ErrorMessage = "A minimum of 5 characters is required")]
        public string FolderName { get; set; }
    }
}
