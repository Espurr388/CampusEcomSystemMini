
//logic ghép các COMPONENTS lại với nhauuuuu


import { useEffect, useState } from "react";

import LoginForm from "./components/LoginForm";
import RegisterForm from "./components/RegisterForm";
import HomePage from "./components/HomePage";
import UserProfile from "./components/UserProfile";
import EditProfile from "./components/EditProfile";
import ChangePassword from "./components/ChangePassword";
import ConnectionRequests from "./components/messenger/ConnectionRequests";

import {
  getMe,
  getToken,
  login,
  logout,
  register,
} from "./services/authService";

import {
  getMe as getUserProfile,
  updateProfile,
  updateAvatar,
  changePassword,
} from "./services/userService";

import "./styles/auth.css";
import "./styles/messenger.css";

export default function App() {

  const [page, setPage] = useState("login");

  const [user, setUser] = useState(null);

  const [loading, setLoading] = useState(false);

  const [error, setError] = useState("");

  const [success, setSuccess] = useState("");


  // =====================================================
  // KIỂM TRA JWT KHI MỞ / REFRESH TRANG
  // =====================================================

  useEffect(() => {

    async function restoreLogin() {

      const token = getToken();

      // Không có token
      if (!token) {
        return;
      }

      setLoading(true);
      setError("");

      try {

        const currentUser = await getMe();

        setUser(currentUser);

        // Có token + /me thành công
        // => vào trang chủ
        setPage("home");

      } catch (error) {

        console.error(
          "Restore login failed:",
          error
        );

        setUser(null);

        setPage("login");

      } finally {

        setLoading(false);

      }
    }

    restoreLogin();

  }, []);


  // =====================================================
  // LOGIN
  // =====================================================

  async function handleLogin(credentials) {

    setLoading(true);

    setError("");

    setSuccess("");

    try {

      // 1. Gọi POST /api/auth/login
      const loginResult =
        await login(credentials);

      console.log(
        "Login response:",
        loginResult
      );


      // 2. Login thành công
      // authService đã lưu JWT vào localStorage


      // 3. Gọi GET /api/auth/me
      const currentUser =
        await getMe();


      // 4. Lưu user vào React state
      setUser(currentUser);


      // 5. CHUYỂN SANG TRANG CHỦ
      setPage("home");


    } catch (error) {

      console.error(
        "Login error:",
        error
      );

      setUser(null);

      setError(
        error.message ||
        "Đăng nhập thất bại."
      );

    } finally {

      setLoading(false);

    }
  }


  // =====================================================
  // REGISTER
  // =====================================================

  async function handleRegister(formData) {

    setLoading(true);

    setError("");

    setSuccess("");

    try {

      // POST /api/auth/register
      await register(formData);


      // Đăng ký thành công
      setSuccess(
        "Đăng ký thành công! Hãy đăng nhập bằng tài khoản vừa tạo."
      );


      // Chuyển về LOGIN
      setPage("login");


    } catch (error) {

      console.error(
        "Register error:",
        error
      );

      setError(
        error.message ||
        "Đăng ký thất bại."
      );

    } finally {

      setLoading(false);

    }
  }


  // =====================================================
  // LOGOUT
  // =====================================================

  async function handleLogout() {

    setLoading(true);

    setError("");

    try {

      await logout();

    } catch (error) {

      console.error(
        "Logout error:",
        error
      );

    } finally {

      // Xóa user khỏi React
      setUser(null);

      // Quay lại Login
      setPage("login");

      setSuccess("");

      setLoading(false);

    }
  }


  // =====================================================
  // CHUYỂN LOGIN
  // =====================================================

  function showLogin() {

    setPage("login");

    setError("");

    setSuccess("");

  }


  // =====================================================
  // CHUYỂN REGISTER
  // =====================================================

  function showRegister() {

    setPage("register");

    setError("");

    setSuccess("");

  }


  // =====================================================
  // CHUYỂN TRANG CHỦ
  // =====================================================

  function showHome() {

    setPage("home");

    setError("");

    setSuccess("");

  }


  // =====================================================
  // KẾT NỐI (MODULE 5)
  // =====================================================

  function showConnections() {

    setPage("connections");

    setError("");

    setSuccess("");

  }


  // =====================================================
  // PROFILE
  // =====================================================

  async function fetchProfile() {

    setLoading(true);

    setError("");

    try {

      const profile = await getUserProfile();

      setUser(profile);

    } catch (error) {

      console.error(

        "Fetch profile failed:",

        error

      );

      setError(

        error.message ||

        "Không thể tải hồ sơ."

      );

    } finally {

      setLoading(false);

    }
  }


  function showProfile() {

    setPage("profile");

    setError("");

    setSuccess("");

    fetchProfile();

  }


  function showEditProfile() {

    setPage("edit-profile");

    setError("");

    setSuccess("");

  }


  function showChangePassword() {

    setPage("change-password");

    setError("");

    setSuccess("");

  }


  // =====================================================
  // UPDATE PROFILE
  // =====================================================

  async function handleUpdateProfile(formData) {

    setLoading(true);

    setError("");

    setSuccess("");

    try {

      // Cập nhật thông tin cá nhân
      const updated = await updateProfile({

        fullName: formData.fullName,

        email: formData.email,

        phone: formData.phone,

      });

      // Cập nhật avatar nếu có URL mới
      if (formData.avatarUrl) {

        await updateAvatar({

          avatarUrl: formData.avatarUrl,

        });

        updated.avatarUrl = formData.avatarUrl;

      }

      // Cập nhật React state
      setUser(updated);

      setSuccess("Hồ sơ đã được cập nhật.");

      setPage("profile");

    } catch (error) {

      console.error(

        "Update profile error:",

        error

      );

      setError(

        error.message ||

        "Cập nhật hồ sơ thất bại."

      );

    } finally {

      setLoading(false);

    }
  }


  // =====================================================
  // CHANGE PASSWORD
  // =====================================================

  async function handleChangePassword(formData) {

    setLoading(true);

    setError("");

    setSuccess("");

    try {

      await changePassword({

        currentPassword: formData.currentPassword,

        newPassword: formData.newPassword,

      });

      setSuccess("Mật khẩu đã được thay đổi.");

      setPage("profile");

    } catch (error) {

      console.error(

        "Change password error:",

        error

      );

      setError(

        error.message ||

        "Đổi mật khẩu thất bại."

      );

    } finally {

      setLoading(false);

    }
  }


  // =====================================================
  // RENDER
  // =====================================================

  return (

    <>

      {/* ================================================
          TRANG ĐĂNG NHẬP
      ================================================= */}

      {page === "login" && (

        <main className="auth-page">

          <div className="auth-background-shape shape-one" />

          <div className="auth-background-shape shape-two" />


          <header className="site-header">

            <div className="brand">

              <span className="brand-icon">
                C
              </span>

              <span>
                Campus
                <span className="brand-highlight">
                  Ecom
                </span>
              </span>

            </div>

            <span className="header-label">
              STUDENT COMMUNITY
            </span>

          </header>


          <section className="auth-layout">

            {/* LEFT */}

            <div className="welcome-panel">

              <span className="eyebrow">
                CAMPUS LIFE, CONNECTED
              </span>

              <h1>
                Kết nối sinh viên.
                <br />

                <span>
                  Chia sẻ cơ hội.
                </span>
              </h1>

              <p>
                Một không gian dành cho sinh viên
                để tìm bạn học, chia sẻ tài liệu
                và kết nối cuộc sống trong khuôn viên trường.
              </p>

            </div>


            {/* RIGHT */}

            <div className="auth-panel">

              {loading && (

                <div className="loading-banner">
                  Đang xử lý...
                </div>

              )}


              {/* LOGIN */}

              {page === "login" && (

                <LoginForm
                  onLogin={handleLogin}
                  onSwitchToRegister={showRegister}
                  loading={loading}
                  error={error}
                />

              )}

            </div>

          </section>


          {success && (

            <div className="message message-success auth-success-message">
              {success}
            </div>

          )}


          <footer className="site-footer">
            CampusEcomSystemMini · Student & Campus Utility
          </footer>

        </main>

      )}


      {/* ================================================
          TRANG ĐĂNG KÝ
      ================================================= */}

      {page === "register" && (

        <main className="auth-page">

          <div className="auth-background-shape shape-one" />

          <div className="auth-background-shape shape-two" />


          <header className="site-header">

            <div className="brand">

              <span className="brand-icon">
                C
              </span>

              <span>
                Campus
                <span className="brand-highlight">
                  Ecom
                </span>
              </span>

            </div>

            <span className="header-label">
              STUDENT COMMUNITY
            </span>

          </header>


          <section className="auth-layout">

            {/* LEFT */}

            <div className="welcome-panel">

              <span className="eyebrow">
                JOIN THE COMMUNITY
              </span>

              <h1>

                Tạo tài khoản.
                <br />

                <span>
                  Kết nối campus.
                </span>

              </h1>

              <p>
                Đăng ký tài khoản để bắt đầu
                sử dụng các tiện ích dành cho sinh viên.
              </p>

            </div>


            {/* RIGHT */}

            <div className="auth-panel">

              {loading && (

                <div className="loading-banner">
                  Đang xử lý...
                </div>

              )}


              <RegisterForm
                onRegister={handleRegister}
                onSwitchToLogin={showLogin}
                loading={loading}
                error={error}
                success=""
              />

            </div>

          </section>


          <footer className="site-footer">
            CampusEcomSystemMini · Student & Campus Utility
          </footer>

        </main>

      )}


      {/* ================================================
          TRANG CHỦ
      ================================================= */}
       {page === "home" && user && (

         <HomePage
           user={user}
           onLogout={handleLogout}
           onViewProfile={showProfile}
           onViewConnections={showConnections}
         />

       )}


       {/* ================================================
           TRANG KẾT NỐI (MODULE 5 · BATCH 1)
       ================================================= */}

       {page === "connections" && user && (

         <ConnectionRequests
           currentUserId={user.id ?? user.Id}
           user={user}
           onBackHome={showHome}
           onLogout={handleLogout}
         />

       )}


       {/* ================================================
           TRANG HỒ SƠ
       ================================================= */}

       {(page === "profile" || page === "edit-profile" || page === "change-password") && (

         <main className="auth-page">

           <div className="auth-background-shape shape-one" />

           <div className="auth-background-shape shape-two" />


           <header className="site-header">

             <div className="brand">

               <span className="brand-icon">
                 C
               </span>

               <span>
                 Campus
                 <span className="brand-highlight">
                   Ecom
                 </span>
               </span>

             </div>

             <span className="header-label">
               STUDENT COMMUNITY
             </span>

           </header>


           <section className="auth-layout">

             <div className="welcome-panel">

               <span className="eyebrow">
                 CAMPUS LIFE, CONNECTED
               </span>

               <h1>
                 Tài khoản & hoạt động.
                 <br />

                 <span>
                   Quản lý cá nhân.
                 </span>
               </h1>

               <p>
                 Cập nhật thông tin cá nhân,
                 đổi mật khẩu và quản lý
                 hoạt động của bạn trên
                 CampusEcomSystemMini.
               </p>

             </div>


             <div className="auth-panel">

               {loading && (

                 <div className="loading-banner">
                   Đang xử lý...
                 </div>

               )}


               {/* PROFILE */}

               {page === "profile" && user && (

                 <UserProfile
                   user={user}
                   onLogout={handleLogout}
                   onEditProfile={showEditProfile}
                   onChangePassword={showChangePassword}
                   loading={loading}
                   error={error}
                 />

               )}


               {/* EDIT PROFILE */}

               {page === "edit-profile" && user && (

                 <EditProfile
                   user={user}
                   onSave={handleUpdateProfile}
                   onCancel={showProfile}
                   loading={loading}
                   error={error}
                 />

               )}


               {/* CHANGE PASSWORD */}

               {page === "change-password" && (

                 <ChangePassword
                   onSave={handleChangePassword}
                   onCancel={showProfile}
                   loading={loading}
                   error={error}
                 />

               )}


               {success && (

                 <div className="message message-success auth-success-message">
                   {success}
                 </div>

               )}

             </div>

           </section>


           <footer className="site-footer">
             CampusEcomSystemMini · Student & Campus Utility
           </footer>

         </main>

       )}


     </>

  );
}