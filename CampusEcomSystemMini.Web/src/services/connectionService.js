// connectionService.js — gọi API kết nối (Module 5)
import { request } from "./authService.js";

// API: POST /api/connections/requests
// Gửi lời mời kết nối, người gửi lấy từ JWT
export async function sendConnectionRequest(receiverId) {
  return request("/api/connections/requests", {
    method: "POST",
    body: JSON.stringify({
      receiverId,
    }),
  });
}

// API: GET /api/connections/requests
// Lấy toàn bộ lời mời của người đang đăng nhập (đã nhận + đã gửi)
export async function getConnectionRequests() {
  const data = await request("/api/connections/requests", {
    method: "GET",
  });

  return data?.items ?? [];
}

// API: PUT /api/connections/requests/{id}/accept
// Chỉ người nhận được đồng ý
export async function acceptConnectionRequest(id) {
  return request(`/api/connections/requests/${id}/accept`, {
    method: "PUT",
  });
}

// API: PUT /api/connections/requests/{id}/reject
// Chỉ người nhận được từ chối
export async function rejectConnectionRequest(id) {
  return request(`/api/connections/requests/${id}/reject`, {
    method: "PUT",
  });
}