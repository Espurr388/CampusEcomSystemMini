namespace CampusEcomSystemMini.Application.Posts;

// Chuẩn hóa dữ liệu đầu vào của bài đăng.
internal static class PostInput
{
    public static string? NormalizeType(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }
}