# LexiFlow API - wwwroot Static Files

Thư mục này chứa các static files cho LexiFlow API.

## Cấu trúc thư mục

```
wwwroot/
├── index.html          # Trang chủ API với thông tin tổng quan
├── favicon.ico         # Icon cho website
├── css/
│   └── lexiflow-api.css # Styles tùy chỉnh cho API documentation
└── js/
    └── lexiflow-api.js  # JavaScript enhancement cho Swagger UI
```

## Mục đích

- **index.html**: Trang landing page đẹp mắt với thông tin về API
- **favicon.ico**: Icon hiển thị trên browser tab
- **css/lexiflow-api.css**: Custom styles cho Swagger UI và API documentation
- **js/lexiflow-api.js**: JavaScript để enhance user experience

## Tính năng

1. **Responsive Design**: Tối ưu cho cả desktop và mobile
2. **API Information**: Hiển thị thông tin chi tiết về API endpoints
3. **Enhanced Swagger UI**: Cải thiện giao diện Swagger với custom styling
4. **Keyboard Shortcuts**: Hỗ trợ các phím tắt tiện ích
5. **Performance Tracking**: Theo dõi thời gian response của API calls
6. **Copy-to-Clipboard**: Dễ dàng copy code examples

## Sử dụng

Các static files được serve tự động bởi ASP.NET Core middleware:
- Truy cập `/` để xem trang chủ API
- Truy cập `/swagger` để xem API documentation
- CSS và JS files được load tự động khi cần thiết