import { useState } from "react";

import { POST_TYPES, POST_TYPE_EMPTY } from "./postTypes.js";

// Form đăng / sửa bài đăng.
// post = null  => tạo bài đăng mới
// post = object => sửa bài đăng đã có
export default function PostFormModal({
  post,
  saving,
  error,
  onSubmit,
  onClose,
}) {
  const isEdit = Boolean(post);

  const [title, setTitle] = useState(post?.title ?? "");
  const [content, setContent] = useState(post?.content ?? "");
  const [type, setType] = useState(post?.type ?? "");
  const [validationError, setValidationError] = useState("");

  async function handleSubmit(event) {
    event.preventDefault();

    setValidationError("");

    if (!title.trim()) {
      setValidationError("Vui lòng nhập tiêu đề bài đăng.");
      return;
    }

    if (!content.trim()) {
      setValidationError("Vui lòng nhập nội dung bài đăng.");
      return;
    }

    await onSubmit({
      title: title.trim(),
      content: content.trim(),
      type,
    });
  }

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div
        className="modal-card"
        role="dialog"
        aria-modal="true"
        onClick={(event) => event.stopPropagation()}
      >
        <button
          className="modal-close"
          type="button"
          aria-label="Đóng"
          onClick={onClose}
          disabled={saving}
        >
          ×
        </button>

        <form className="auth-form modal-form" onSubmit={handleSubmit}>
          <div className="form-heading">
            <span className="eyebrow">BÀI ĐĂNG</span>

            <h2>
              {isEdit ? "Sửa bài đăng" : "Đăng bài mới"}
            </h2>

            <p>
              Chia sẻ thông tin với sinh viên trong khuôn viên
              trường.
            </p>
          </div>

          {validationError && (
            <div className="message message-error" role="alert">
              {validationError}
            </div>
          )}

          {error && (
            <div className="message message-error" role="alert">
              {error}
            </div>
          )}

          <div className="form-group">
            <label htmlFor="post-title">Tiêu đề</label>

            <input
              id="post-title"
              type="text"
              placeholder="Thuê & Ở ghép trọ"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="post-type">Nhóm nội dung</label>

            <select
              id="post-type"
              value={type}
              onChange={(e) => setType(e.target.value)}
            >
              {[POST_TYPE_EMPTY, ...POST_TYPES].map((item) => (
                <option
                  key={item.value || "empty"}
                  value={item.value}
                >
                  {item.label}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label htmlFor="post-content">Nội dung</label>

            <textarea
              id="post-content"
              rows={6}
              placeholder="Mô tả chi tiết bài đăng của bạn..."
              value={content}
              onChange={(e) => setContent(e.target.value)}
              required
            />
          </div>

          <div className="posts-actions">
            <button
              className="btn btn-primary"
              type="submit"
              disabled={saving}
            >
              {saving
                ? "Đang lưu..."
                : isEdit
                  ? "Lưu thay đổi"
                  : "Đăng bài"}
            </button>

            <button
              className="btn btn--ghost"
              type="button"
              onClick={onClose}
              disabled={saving}
            >
              Hủy
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}