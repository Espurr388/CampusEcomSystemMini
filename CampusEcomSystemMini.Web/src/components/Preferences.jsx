import { useState } from "react";

// Màn hình "Vector nhu cầu".
// Dữ liệu này được Module 2 dùng cho Smart Matching.
export default function Preferences({
  preferences,
  loading,
  saving,
  error,
  success,
  onSave,
  onDelete,
  onBack,
}) {
  const [interestedSubjects, setInterestedSubjects] = useState(
    preferences?.interestedSubjects ??
      preferences?.InterestedSubjects ??
      ""
  );

  const [habits, setHabits] = useState(
    preferences?.habits ?? preferences?.Habits ?? ""
  );

  const [goals, setGoals] = useState(
    preferences?.goals ?? preferences?.Goals ?? ""
  );

  const [preferredRentalArea, setPreferredRentalArea] = useState(
    preferences?.preferredRentalArea ??
      preferences?.PreferredRentalArea ??
      ""
  );

  const [monthlyRentalBudget, setMonthlyRentalBudget] = useState(
    preferences?.monthlyRentalBudget ??
      preferences?.MonthlyRentalBudget ??
      ""
  );

  const [validationError, setValidationError] = useState("");

  const hasPreferences = Boolean(preferences);

  async function handleSubmit(event) {
    event.preventDefault();

    setValidationError("");

    const hasAnyValue =
      interestedSubjects.trim() ||
      habits.trim() ||
      goals.trim() ||
      preferredRentalArea.trim() ||
      String(monthlyRentalBudget).trim();

    if (!hasAnyValue) {
      setValidationError(
        "Vui lòng điền ít nhất một thông tin của vector nhu cầu."
      );
      return;
    }

    await onSave({
      interestedSubjects: interestedSubjects.trim(),
      habits: habits.trim(),
      goals: goals.trim(),
      preferredRentalArea: preferredRentalArea.trim(),
      monthlyRentalBudget:
        monthlyRentalBudget === "" || monthlyRentalBudget === null
          ? null
          : Number(monthlyRentalBudget),
    });
  }

  function handleDelete() {
    const confirmed = window.confirm(
      "Xóa toàn bộ vector nhu cầu của bạn? Hành động này không thể hoàn tác."
    );

    if (!confirmed) {
      return;
    }

    onDelete();
  }

  if (loading) {
    return (
      <section className="pref-card">
        <div className="pref-loading">Đang tải vector nhu cầu...</div>
      </section>
    );
  }

  return (
    <section className="pref-card">
      <div className="pref-heading">
        <div>
          <span className="eyebrow">SMART MATCHING</span>

          <h1 className="pref-title">Vector nhu cầu</h1>

          <p className="pref-subtitle">
            Mô tả sở thích, thói quen và mục tiêu của bạn để hệ thống
            gợi ý nhóm học tập và phòng trọ phù hợp.
          </p>
        </div>

        <span
          className={
            hasPreferences
              ? "pref-status pref-status--ready"
              : "pref-status pref-status--empty"
          }
        >
          {hasPreferences ? "Đã thiết lập" : "Chưa thiết lập"}
        </span>
      </div>

      {error && (
        <div className="message message-error" role="alert">
          {error}
        </div>
      )}

      {success && (
        <div className="message message-success" role="status">
          {success}
        </div>
      )}

      {validationError && (
        <div className="message message-error" role="alert">
          {validationError}
        </div>
      )}

      <form className="pref-form" onSubmit={handleSubmit}>
        <div className="pref-grid">
          <div className="form-group">
            <label htmlFor="pref-subjects">Môn học / chủ đề quan tâm</label>

            <textarea
              id="pref-subjects"
              rows={4}
              placeholder="Lập trình, Cấu trúc dữ liệu, Marketing"
              value={interestedSubjects}
              onChange={(e) => setInterestedSubjects(e.target.value)}
            />

            <span className="form-hint">
              Mỗi mục một dòng hoặc phân tách bằng dấu phẩy.
            </span>
          </div>

          <div className="form-group">
            <label htmlFor="pref-habits">Thói quen / lối sống</label>

            <textarea
              id="pref-habits"
              rows={4}
              placeholder="Dậy sớm, học theo nhóm, thích làm việc tại kho"
              value={habits}
              onChange={(e) => setHabits(e.target.value)}
            />

            <span className="form-hint">
              Mô tả lịch sinh hoạt và cách bạn học tập.
            </span>
          </div>

          <div className="form-group">
            <label htmlFor="pref-goals">Mục tiêu</label>

            <textarea
              id="pref-goals"
              rows={4}
              placeholder="Tìm nhóm ôn tập lập trình, tìm bạn ở ghép trọ"
              value={goals}
              onChange={(e) => setGoals(e.target.value)}
            />

            <span className="form-hint">
              Những điều bạn muốn đạt được trên campus.
            </span>
          </div>
        </div>

        <div className="pref-grid pref-grid--two">
          <div className="form-group">
            <label htmlFor="pref-area">Khu vực thuê trọ yêu thích</label>

            <input
              id="pref-area"
              type="text"
              placeholder="Quận 1, Bình Thạnh, Thủ Đức"
              value={preferredRentalArea}
              onChange={(e) => setPreferredRentalArea(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="form-group">
            <label htmlFor="pref-budget">
              Ngân sách thuê trọ hàng tháng (VNĐ)
            </label>

            <input
              id="pref-budget"
              type="number"
              min="0"
              step="100000"
              placeholder="3000000"
              value={monthlyRentalBudget}
              onChange={(e) => setMonthlyRentalBudget(e.target.value)}
              autoComplete="off"
            />
          </div>
        </div>

        <div className="pref-actions">
          <button
            className="btn btn-primary"
            type="submit"
            disabled={saving}
          >
            {saving
              ? "Đang lưu..."
              : hasPreferences
                ? "Lưu thay đổi"
                : "Tạo vector nhu cầu"}
          </button>

          <button
            className="btn btn--ghost"
            type="button"
            onClick={onBack}
            disabled={saving}
          >
            Quay lại hồ sơ
          </button>

          {hasPreferences && (
            <button
              className="btn btn-danger"
              type="button"
              onClick={handleDelete}
              disabled={saving}
            >
              {saving ? "Đang xử lý..." : "Xóa vector nhu cầu"}
            </button>
          )}
        </div>
      </form>
    </section>
  );
}