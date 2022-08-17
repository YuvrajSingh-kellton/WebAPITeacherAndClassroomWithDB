namespace WebAPITeacherAndClassroom.ApiModels
{
    public class BindingTeacherAndClassroom
    {
        public TeacherModel? TeacherModel { get; set; }
        public List<ClassroomModel>? ClassroomModelList { get; set; }
    }
}
