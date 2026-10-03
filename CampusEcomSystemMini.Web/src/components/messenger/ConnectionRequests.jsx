// ConnectionRequests.jsx — Lời mời kết nối (Module 5 · Batch 1)
import { useCallback, useEffect, useState } from "react";

import {
  acceptConnectionRequest,
  getConnectionRequests,
  rejectConnectionRequest,
  sendConnectionRequest,
} from "../../services/connectionService";

const GUID_PATTERN =
  /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;

const STATUS_META = {
  Pending: {
    label: "Đang chờ",
    className: "connection-status--pending",
  },
  Accepted: {
    label: "Đã đồng ý",
    className: "connection-status--accepted",
  },
  Rejected: {
    label: "Đã từ chối",
    className: "connection-status--rejected",
  },
};

function getInitials(fullName) {
  return (
    fullName
      ?.trim()
      .split(/\s+/)
      .map((part) => part[0])
      .slice(-2)
      .join("")
      .toUpperCase() || "SV"
  );
}

function formatDateTime(value) {
  if (!value) {
    return "";
  }

  // SQL Server trả về datetime2 không kèm múi giờ,
  // coi như UTC để hiển thị đúng giờ Việt Nam.
  const hasZone = /(?:Z|[+-]\d{2}:\d{2})$/.test(value);

  const date = new Date(hasZone ? value : `${value}Z`);

  if (Number.isNaN(date.getTime())) {
    return "";
  }

  return date.toLocaleString("vi-VN", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
}

export default function ConnectionRequests({
  currentUserId,
  user,
  onBackHome,
  onLogout,
}) {
  const [requests, setRequests] = useState([]);
  const [tab, setTab] = useState("received");
  const [receiverId, setReceiverId] = useState("");

  const [loading, setLoading] = useState(true);
  const [sending, setSending] = useState(false);
  const [processingId, setProcessingId] = useState("");

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const loadRequests = useCallback(async () => {
    try {
      const items = await getConnectionRequests();

      setRequests(items);
      setError("");
    } catch (loadError) {
      console.error("Load connection requests failed:", loadError);

      setError(
        loadError.message ||
          "Không thể tải danh sách lời mời kết nối."
      );
    } finally {
      setLoading(false);
    }
  }, []);

  const refreshRequests = useCallback(async () => {
    setLoading(true);

    await loadRequests();
  }, [loadRequests]);

  useEffect(() => {
    let active = true;

    getConnectionRequests()
      .then((items) => {
        if (active) {
          setRequests(items);
        }
      })
      .catch((loadError) => {
        console.error(
          "Load connection requests failed:",
          loadError
        );

        if (active) {
          setError(
            loadError.message ||
              "Không thể tải danh sách lời mời kết nối."
          );
        }
      })
      .finally(() => {
        if (active) {
          setLoading(false);
        }
      });

    return () => {
      active = false;
    };
  }, []);

  const handleSend = async (event) => {
    event.preventDefault();

    const value = receiverId.trim();

    if (!GUID_PATTERN.test(value)) {
      setSuccess("");
      setError("Mã tài khoản không hợp lệ.");

      return;
    }

    setSending(true);
    setError("");
    setSuccess("");

    try {
      await sendConnectionRequest(value);

      setSuccess("Đã gửi lời mời kết nối.");
      setReceiverId("");
      setTab("sent");

      await refreshRequests();
    } catch (sendError) {
      console.error("Send connection request failed:", sendError);

      setError(
        sendError.message || "Gửi lời mời kết nối thất bại."
      );
    } finally {
      setSending(false);
    }
  };

  const handleAccept = async (id) => {
    setProcessingId(id);
    setError("");
    setSuccess("");

    try {
      await acceptConnectionRequest(id);

      setSuccess("Đã đồng ý kết nối.");

      await refreshRequests();
    } catch (acceptError) {
      console.error("Accept connection request failed:", acceptError);

      setError(
        acceptError.message || "Đồng ý kết nối thất bại."
      );
    } finally {
      setProcessingId("");
    }
  };

  const handleReject = async (id) => {
    setProcessingId(id);
    setError("");
    setSuccess("");

    try {
      await rejectConnectionRequest(id);

      setSuccess("Đã từ chối lời mời kết nối.");

      await refreshRequests();
    } catch (rejectError) {
      console.error("Reject connection request failed:", rejectError);

      setError(
        rejectError.message || "Từ chối kết nối thất bại."
      );
    } finally {
      setProcessingId("");
    }
  };

  const currentId = (currentUserId ?? "").toLowerCase();

  const receivedRequests = requests.filter(
    (item) =>
      (item.receiverId ?? "").toLowerCase() === currentId
  );

  const sentRequests = requests.filter(
    (item) => (item.senderId ?? "").toLowerCase() === currentId
  );

  const visibleRequests =
    tab === "received" ? receivedRequests : sentRequests;

  const fullName = user?.fullName ?? user?.FullName ?? "Sinh viên";
  const email = user?.email ?? user?.Email ?? "";

  return (
    <div className="connection-page">

      {/* HEADER */}
      <header className="connection-header">

        <div className="connection-brand">
          <div className="connection-brand-icon">
            C
          </div>

          <div>
            <strong>
              Campus<span>Ecom</span>
            </strong>

            <small>
              Messenger
            </small>
          </div>
        </div>

        <div className="connection-header-right">

          <button
            className="connection-ghost-button"
            type="button"
            onClick={onBackHome}
          >
            Về trang chủ
          </button>

          <div className="connection-user">
            <div className="connection-user-avatar">
              {getInitials(fullName)}
            </div>

            <div className="connection-user-info">
              <strong>{fullName}</strong>
              <span>{email}</span>
            </div>

            <button
              className="connection-ghost-button"
              type="button"
              onClick={onLogout}
            >
              Đăng xuất
            </button>
          </div>

        </div>

      </header>


      {/* MAIN */}
      <main className="connection-main">

        <section className="connection-hero">
          <span className="eyebrow">
            MESSENGER · KẾT NỐI
          </span>

          <h1>
            Lời mời kết nối<span>.</span>
          </h1>

          <p>
            Gửi lời mời, đồng ý hoặc từ chối kết nối
            với sinh viên khác trong CampusEcomSystemMini.
          </p>
        </section>


        {/* GỬI LỜI MỜI */}
        <section className="connection-card">

          <div className="connection-card-heading">
            <h2>Gửi lời mời kết nối</h2>
            <p>
              Nhập mã tài khoản (GUID) của sinh viên
              bạn muốn kết nối.
            </p>
          </div>

          <form
            className="connection-send-form"
            onSubmit={handleSend}
          >
            <input
              className="connection-input"
              type="text"
              placeholder="Ví dụ: 7288290b-50b3-45b4-8047-3a824ae1a9a5"
              value={receiverId}
              onChange={(event) =>
                setReceiverId(event.target.value)
              }
              autoComplete="off"
              spellCheck={false}
            />

            <button
              className="connection-primary-button"
              type="submit"
              disabled={sending}
            >
              {sending ? "Đang gửi..." : "Kết nối"}
            </button>
          </form>

        </section>


        {/* DANH SÁCH LỜI MỜI */}
        <section className="connection-card">

          <div className="connection-card-heading connection-card-heading--row">
            <div>
              <h2>Lời mời kết nối</h2>
              <p>
                Quản lý các lời mời bạn nhận và đã gửi.
              </p>
            </div>

            <div className="connection-tabs">

              <button
                className={
                  tab === "received"
                    ? "connection-tab connection-tab--active"
                    : "connection-tab"
                }
                type="button"
                onClick={() => setTab("received")}
              >
                Đã nhận
                <span>{receivedRequests.length}</span>
              </button>

              <button
                className={
                  tab === "sent"
                    ? "connection-tab connection-tab--active"
                    : "connection-tab"
                }
                type="button"
                onClick={() => setTab("sent")}
              >
                Đã gửi
                <span>{sentRequests.length}</span>
              </button>

            </div>
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

          {loading && (
            <div className="connection-state">
              Đang tải lời mời kết nối...
            </div>
          )}

          {!loading && visibleRequests.length === 0 && (
            <div className="connection-state connection-state--empty">
              {tab === "received"
                ? "Bạn chưa nhận được lời mời kết nối nào."
                : "Bạn chưa gửi lời mời kết nối nào."}
            </div>
          )}

          {!loading && visibleRequests.length > 0 && (
            <ul className="connection-list">
              {visibleRequests.map((item) => {
                const isReceived =
                  tab === "received";

                const fullNameOfOtherUser = isReceived
                  ? item.senderFullName
                  : item.receiverFullName;

                const emailOfOtherUser = isReceived
                  ? item.senderEmail
                  : item.receiverEmail;

                const statusMeta = STATUS_META[item.status] ?? {
                  label: item.status,
                  className: "connection-status--pending",
                };

                const canAct =
                  isReceived &&
                  item.status === "Pending";

                const isProcessing =
                  processingId === item.id;

                return (
                  <li className="connection-item" key={item.id}>

                    <div className="connection-item-avatar">
                      {getInitials(fullNameOfOtherUser)}
                    </div>

                    <div className="connection-item-body">
                      <div className="connection-item-top">
                        <strong>
                          {fullNameOfOtherUser || "Sinh viên"}
                        </strong>

                        <span
                          className={`connection-status ${statusMeta.className}`}
                        >
                          {statusMeta.label}
                        </span>
                      </div>

                      <span className="connection-item-email">
                        {emailOfOtherUser}
                      </span>

                      <span className="connection-item-time">
                        {isReceived
                          ? `Nhận lúc ${formatDateTime(item.createdAt)}`
                          : `Gửi lúc ${formatDateTime(item.createdAt)}`}
                      </span>
                    </div>

                    {canAct ? (
                      <div className="connection-item-actions">
                        <button
                          className="connection-action connection-action--accept"
                          type="button"
                          onClick={() => handleAccept(item.id)}
                          disabled={loading || isProcessing}
                        >
                          Đồng ý
                        </button>

                        <button
                          className="connection-action connection-action--reject"
                          type="button"
                          onClick={() => handleReject(item.id)}
                          disabled={loading || isProcessing}
                        >
                          Từ chối
                        </button>
                      </div>
                    ) : (
                      <span className="connection-item-hint">
                        {tab === "received"
                          ? "Đã xử lý"
                          : "Chờ phản hồi"}
                      </span>
                    )}

                  </li>
                );
              })}
            </ul>
          )}

        </section>

      </main>


      {/* FOOTER */}
      <footer className="connection-footer">
        CampusEcomSystemMini · Student & Campus Utility
      </footer>

    </div>
  );
}