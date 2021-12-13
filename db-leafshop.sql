CREATE DATABASE LeafShop 
GO
USE LeafShop

----------------------TẠO BẢNG----------------------

--Tạo bảng Thể loại--
GO
CREATE TABLE DanhMuc
(
	MaDanhMuc INT IDENTITY NOT NULL PRIMARY KEY,
	TenDanhMuc NVARCHAR(100) NOT NULL,
	ParentId int NULL 
)
GO
ALTER TABLE DanhMuc add foreign key (ParentId) references DanhMuc(MaDanhMuc)
GO
--Tạo bảng Nhà sản xuất--
GO
CREATE TABLE ThuongHieu
(
	MaThuongHieu INT IDENTITY NOT NULL  PRIMARY KEY,
	TenThuongHieu NVARCHAR(100) NOT NULL,
	DiaChiThuongHieu NVARCHAR(100) NULL,
	DienThoaiThuongHieu VARCHAR(20) NULL,
	MoTaThuongHieu nvarchar(2000) NULL,
	AnhThuongHieu VARCHAR(1000) NULL
)

--Tạo bảng Nhân viên--
GO
CREATE TABLE NhanVien
(
	MaNhanVien INT NOT NULL IDENTITY PRIMARY KEY,
	TenNhanVien NVARCHAR(100) NOT NULL ,
	GioiTinh BIT NULL,
	Avatar NVARCHAR(1000) NULL,
	Email NVARCHAR(50) NOT NULL,
	NgaySinh DATE NOT NULL,
	DienThoai VARCHAR(20) not null,
	DiaChi NVARCHAR(500) not null
)


--Tạo bảng Sản phẩm--
GO
CREATE TABLE SanPham
(
	MaSanPham Int identity not null,
	TenSanPham NVARCHAR(500)NOT NULL,
	MaDanhMuc INT ,
	MaThuongHieu INT ,
	DonViTinh NVARCHAR(50) NULL,
	SoLuong INT NOT NULL,
	SoLuongBan INT Default 0 NOT NULL,
	DonGia INT NOT NULL ,
	MoTa nvarchar(4000) NOT NULL,
	NgayKhoiTao Date NULL,
	NgayCapNhat Date NULL,
	HinhMinhHoa VARCHAR(1000) NULL,
	constraint PK_sanpham primary key (MaSanPham),
	constraint PK_sanpham1 foreign key (MaDanhMuc) REFERENCES DanhMuc(MaDanhMuc),
	constraint PK_sanpham3 foreign key (MaThuongHieu) REFERENCES ThuongHieu(MaThuongHieu)
)

GO
CREATE TABLE KhachHang
(
	MaKhachHang INT Identity NOT NULL PRIMARY KEY,
	TenKhachHang NVARCHAR(50) NOT NULL ,
	DiaChiKhachHang NVARCHAR(100)NULL,
	DienThoaiKhachHang VARCHAR(20) NULL,
	TenDangNhap VARCHAR(50) NOT NULL,
	MatKhau VARCHAR(50) NOT NULL,
	NgaySinh date NULL,
	GioiTinh BIT NULL,
	Email VARCHAR(50) NULL,
	TrangThai bit DEFAULT 1
)

GO
CREATE TABLE DanhMucBlog
(
  MaDanhMucBlog INT IDENTITY NOT NULL PRIMARY KEY,
  TenDanhMucBlog NVARCHAR(100) NOT NULL,
)

--Tạo bảng Bài Viết
GO
CREATE TABLE Blog(
	MaBaiViet INT IDENTITY NOT NULL,
	MaNhanVien Int,
	MaDanhMucBlog int,
	TieuDe nvarchar(500) NOT NULL,
	Anh VARCHAR(1000) NULL,
	Tomtat nvarchar(500) NOT NULL,
	Noidung nvarchar(2000) NOT NULL,
	NgayKhoiTao Date null,
	constraint PK_Blog primary key (MaBaiViet),
	constraint PK_Blog1 foreign key (MaNhanVien) REFERENCES NhanVien(MaNhanVien),
	constraint PK_Blog2 foreign key (MaDanhMucBlog) REFERENCES DanhMucBlog(MaDanhMucBlog)
)

--Tạo bảng Tài khoản
GO
CREATE TABLE Taikhoan(
	USERNAME nvarchar(50) primary key not null,
	PASSWORD nvarchar(50) ,
	Quantri bit,
	MaNhanVien Int,
	constraint PK_TaiKhoan foreign key (MaNhanVien) REFERENCES NhanVien(MaNhanVien)
)

--Tạo bảng Đặt hàng 
GO
CREATE TABLE DatHang(
	MaDatHang INT NOT NULL IDENTITY,
	MaKhachHang INT, 
	MaNhanVien INT null,
	TongTien Int,
	NgayKhoiTao Date not null,
	NgayGiaoHang Date NULL,
	GhiChu nvarchar(1000) null,
	DiaChi nvarchar(500) null,
	TrangThai BIT not null,
	constraint PK_DH primary key (MaDatHang),
	constraint PK_DH1 FOREIGN KEY (MaKhachHang)  REFERENCES KhachHang(MaKhachHang),
	constraint PK_DH2 FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien)
)

--Tạo bảng Chi tiết đặt hàng--
GO
CREATE TABLE ChiTietDatHang
(
	MaDatHang INT not null,
	MaSanPham INT not null,
	SoLuong INT NULL,
	DonGia INT NULL,
	constraint PK_CTDH primary key (MaDatHang, MaSanPham),
	constraint PK_CTDH1 foreign key (MaDatHang) references DatHang(MaDatHang),
	constraint PK_CTDH2 foreign key (MaSanPham) references SanPham(MaSanPham)
)


--Thêm dữ liệu vào bảng danh mục--
GO
INSERT INTO DanhMuc VALUES(N'Thực phẩm tươi sống',null)
INSERT INTO DanhMuc VALUES(N'Thực phẩm khô',null)
INSERT INTO DanhMuc VALUES(N'Làm đẹp',null)
INSERT INTO DanhMuc VALUES(N'Chăm sóc cơ thể',null)
INSERT INTO DanhMuc VALUES(N'Mẹ & bé',null)
INSERT INTO DanhMuc VALUES(N'Chăm sóc nhà cửa',null)
INSERT INTO DanhMuc VALUES(N'Đời sống & tinh thần',null)
INSERT INTO DanhMuc VALUES(N'Zero waste',null)
INSERT INTO DanhMuc VALUES(N'Sale',null)
INSERT INTO DanhMuc VALUES(N'Thịt và thủy hải sản',1)
INSERT INTO DanhMuc VALUES(N'Rau, củ quả tươi',1)
INSERT INTO DanhMuc VALUES(N'Trái cây tươi',1)
INSERT INTO DanhMuc VALUES(N'Đồ uống',2)
INSERT INTO DanhMuc VALUES(N'Gạo hữu cơ',2)
INSERT INTO DanhMuc VALUES(N'Trang điểm',3)
INSERT INTO DanhMuc VALUES(N'Chăm sóc da mặt',3)
INSERT INTO DanhMuc VALUES(N'Thịt, xương và sản phẩm từ lợn',10)
INSERT INTO DanhMuc VALUES(N'Gia cầm và trứng',10)


--Thêm dữ liệu vào bảng ThuongHieu--
GO
INSERT INTO ThuongHieu VALUES(N'BUCCOTHERM',N'Hà Nội','0123456789', N'Dược sĩ Jean-Jacques Lascombes, người sáng lập thương hiệu Buccotherm chuyên về sản phẩm chăm sóc răng miệng, đã luôn quan tâm đặc biệt đến nguồn nước khoáng nóng Castéra-Verduzan.Cuộc gặp gỡ với một chuyên gia nha sĩ đã giúp ông quyết tâm với lựa chọn của mình. Trải qua nhiều cuộc nghiên cứu và thử nghiệm khoa học, Lascombes đã chứng minh được đặc tính trị liệu của nước khoáng nóng Castéra-Verduzan đối với hệ răng miệng. Ông đã nghĩ ra phương pháp và quy trình để thu được nước khoáng nóng Castéra-Verduzan với độ kiềm ổn định pH8, là thành phần chính trong dòng sản phẩm chăm sóc răng miệng BUCCOTHERM (Bucco = răng miệng ; Therm = khoáng nóng). Sử dụng sản phẩm Buccotherm, bạn sẽ không tìm thấy các thành phần gây tranh cãi hay ảnh hưởng đến sức khoẻ như paraben, sodium lauryl sulphate, saccharine hay hương liệu nhân tạo. Đến với Buccotherm, bạn có thể yên tâm sử dụng sản phẩm chăm sóc răng miệng cho bản thân và gia đình.', '/Areas/UploadFile/ThuongHieu/thuonghieu1.jpg')
INSERT INTO ThuongHieu VALUES(N'Markal',N'Hà Nội','0123456789', N'Nằm trong khu nông nghiệp hữu cơ của vùng  La Drome miền Nam nước Pháp, Markal được  biết đến như một trong những nhà cung cấp  chất lượng hàng đầu các sản phẩm từ ngũ cốc  và gạo. Thành lập từ năm 1936 với sản phẩm    duy nhất là tấm lúa mỳ rang (bulgur), đến nay  Markal đáp ứng hàng ngàn sản phẩm qua các  hệ thống phân phối hơn 900 cửa hàng hữu cơ  trên toàn nước Pháp. Với hệ thống sản xuất và quy trình kiểm định  chặt chẽ theo chuẩn hữu cơ Châu Âu và Pháp,  Markal tự hào mang đến cho người tiêu dùng  những sản phẩm lành mạnh và an toàn nhất. ', '/Areas/UploadFile/ThuongHieu/thuonghieu2.jpg')
INSERT INTO ThuongHieu VALUES(N'Daioni',N'Đà Nẵng','0123456789',N'Daioni, nhãn hiệu sữa đạt chứng nhận tiêu chuẩn Organic nổi tiếng của xứ Wales - Anh Quốc đang được phân phối chính thức tại Việt Nam! Tất cả các sản phẩm của Daioni đều được làm từ 100% sữa tươi hữu cơ và được chứng nhận chất lượng bởi một trong những Tổ chức chứng nhận hữu cơ có thẩm quyền lớn nhất của Liên minh châu Âu - The Soil Association.', '/Areas/UploadFile/ThuongHieu/thuonghieu3.jpg')
INSERT INTO ThuongHieu VALUES(N'Alvins',N'Hồ Chí Minh','0123456789',N'Alvins là thương hiệu thực phẩm hỗ trợ quá trình tăng trưởng và phát triển trí não của trẻ thông qua nghiên cứu từ các chuyên gia dinh dưỡng hàng đầu.Thành lập từ năm 2007, với giá cả hợp lý, mạng lưới phân phối và đặt hàng rộng lớn, tiện lợi khi bảo quản, Alvins giúp tiết kiệm thời gian chế biến đồ ăn cho trẻ mà không cần lo lắng về thành phần dinh dưỡng.Được làm từ nguyên liệu tươi ngon, quy trình chế biến đạt chuẩn HACCP, Alvins tự hào là thương hiệu đồng hành cùng mẹ và bé.', '/Areas/UploadFile/ThuongHieu/thuonghieu4.jpg')
INSERT INTO ThuongHieu VALUES(N'AVRIL',N'Đà Nẵng','0123456789',N'Avril là một thương hiệu tuy mới ra đời từ năm 2012 nhưng đến nay đã trở nên quen thuộc và được đông đảo người tiêu dùng mỹ phẩm trang điểm tự nhiên và hữu cơ tin tưởng sử dụng. Thành công của thương hiệu tập trung ở 3 thế mạnh: chất lượng, tôn trọng môi trường sinh thái và giá cả cạnh tranh.', '/Areas/UploadFile/ThuongHieu/thuonghieu5.jpg')

--Thêm dữ liệu vào bảng Khách hàng--

GO
INSERT INTO KhachHang VALUES(N'Đỗ Bá Hoàn',N'Hà Nội','0912345678','allfallsdown','dobahoan','01/01/1990','1','dbh@gmail.com',1)
INSERT INTO KhachHang VALUES(N'Quách Ngọc Hà',N'Đà Nẵng','0912345678','lenovoquach','quachngocha','01/01/1990','1','lenovo@gmail.com',1)
INSERT INTO KhachHang VALUES(N'Nguyễn Tiến Hà',N'Hồ Chí Minh','0912345678','damchem','hanguyen','01/01/1990','1','hanguyentien2000@gmail.com',1)

--------Thêm dữ liệu vào bảng Sản phẩm--
INSERT INTO SanPham VALUES(N'Hạt chia hữu cơ Markal 250g','14','2',N'Túi','5','10','145000',N'<strong>Thành phần</strong> : 100% hạt chia hữu cơ.<br><strong>Đặc điểm :</strong><br>- Giàu omega- 3 : giúp kiểm soát lượng mỡ máu,&nbsp;hệ thống tim mạch khỏe mạnh, giảm nguy cơ mắc bệnh tim, cải thiện trí nhớ, huyết áp cao, tăng khả năng tập trung khi làm việc, giúp sáng mắt và làm giảm chứng khô mắt, mỏi mắt...<br><span style="color: #1d2129;">- Giàu protein: hàm lượng protein trong hạt chia thường cao gấp đôi so với các loại hạt ngũ cốc khác. Đặc biệt axit amin Tryptophan có trong hạt chia là&nbsp;</span>tiền thân của một số hormone quan trọng,&nbsp;bao gồm chất dẫn truyền thần kinh serotonin.&nbsp;Serotonin&nbsp; giúp làm bận thư giãn, giảm triệu chứng lo âu, sản xuất ra Melatonin để bạn có thể ngủ tốt hơn.<br>-&nbsp;Giàu canxi: hỗ trợ củng cố hệ xương và răng.<br><span style="font-family: Helvetica, Arial, sans-serif;">- Giàu chất sắt : thành phần có trong các tế bào hồng cầu, hỗ trợ vận chuyển oxi đến các mô</span><span style="font-family: arial, helvetica, sans-serif;">,&nbsp;duy trì sự khoẻ mạnh của hệ miễn dịch, các cơ và điều chỉnh sự phát triển của các tế bào.<br></span>- Giàu chất xơ :giúp nhuận tràng, hạn chế táo bón, hỗ trợ lợi khuẩn trong đường ruột.','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham1.jpg')
INSERT INTO SanPham VALUES(N'Xịt thơm miệng hữu cơ Buccotherm 15ml','4','1',N'Chai','0','10','129000',N'<div>&nbsp;</div><div><strong>Công dụng:</strong></div><div>Giúp ngăn ngừa và loại bỏ các mảng bám trên răng, phòng ngừa sâu răng</div><div>Loại bỏ và giảm thiểu vi khuẩn&nbsp;</div><div>Mang đến một hơi thở tươi mát</div><div>Sản phẩm dành cho người lớn. Thích hợp với những người bị hôi miệng, những người muốn có một hơi thở thơm tho.</div><div>Cách sử dụng: Mỗi lần dùng 1-2 xịt, sử dụng 3-4 lần/ngày</div><div>&nbsp;</div><div><strong>Thành phần :</strong></div><div>Castera-Verduzan Thermal Aqua,&nbsp;alcohol*, xylitol, glycerin*, hydrogenated starch hydrolysate, water, aroma, mentha piperita water**, camellia sinensis leaf water**, dehydroacetic acid, benzyl alcohol, limonene.</div><div>&nbsp;</div><div><strong><em>Sản xuất tại Pháp và chứng nhận hữu cơ bởi Ecocert.</em></strong></div>','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham2.jpg')
INSERT INTO SanPham VALUES(N'Nước súc miệng hữu cơ Buccotherm 300ml','4','1',N'Bình','5','10','199000',N'<div>Với các hoạt chất làm sạch diệt khuẩn nguồn gốc tự nhiên, nước súc miệng hữu cơ Buccotherm mang đến các công dụng:</div><div>- Giúp diệt khuẩn, ngăn ngừa vi khuẩn, vi rút lây lan qua đường răng miệng</div><div>- Giúp phòng tránh, ngăn ngừa các bệnh về răng miệng như viêm lợi, sâu răng</div><div>- Giúp làm sạch các mảng bám thức ăn, vệ sinh răng miệng hàng ngày,</div><div>- Hỗ trợ điều trị, ngăn ngừa bệnh hôi miệng hiệu quả</div><div>- Tạo sự tư tin với nụ cười tỏa sáng và hơi thở thơm tho suốt cả ngày dài.</div><div>Sản phẩm không chứa fluoride và sulfate.</div><div>Phù hợp với người lớn. Sản phẩm không dùng cho trẻ dưới 7 tuổi</div><div style="text-align: justify;">','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham3.jpg')
INSERT INTO SanPham VALUES(N'Kem đánh răng hữu cơ than hoạt tính Buccotherm 75ml','4','1',N'Tuýp','0','10','195000',N'Phù hợp với người lớn đang tìm kiếm một liệu pháp làm trắng răng hoàn chỉnh để sử dụng hàng ngày<br>- Than hoạt tính giúp làm trắng răng, tẩy các vết ố trên răng, loại bỏ và làm sạch cặn thức ăn gây đổi màu răng, giúp chống lại các mảng bám.<br>- Bổ sung hương thơm bạc hà 100% tự nhiên cho cảm giác tươi mát<br>- Nhờ bổ sung nước suối khoáng nóng Castéra-Verduzan, giàu muối<br>khoáng và các nguyên tố vi lượng, giúp làm dịu và tái khoáng hiệu quả.','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham4.jpg')
INSERT INTO SanPham VALUES(N'Sữa gạo hữu cơ không đường 4CARE BALANCE 1L','13','3',N'Hộp','5','10','89000',N'Đây là sữa','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham5.jpg')
INSERT INTO SanPham VALUES(N'Bánh gạo lứt ăn dặm hữu cơ cho bé vị bí ngô Alvins 25g','5','5',N'Gói','5','10','72000',N'Dành cho trẻ em','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham6.jpg')
INSERT INTO SanPham VALUES(N'Sữa gạo hữu cơ vị vani 4Care Balance 180ml','13','3',N'Hộp','5','10','24000',N'Sữa gạo hữu cơ Balance an toàn cho mọi lứa tuổi. Chúng tôi khuyên các bé bú mẹ trong giai đoạn đầu đời. Sữa gạo hữu cơ Balance có thể được bổ sung với sữa mẹ và các loại thức ăn dặm khác cho các bé từ 6','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham7.jpg')
INSERT INTO SanPham VALUES(N'Chì kẻ mắt hữu cơ Avril màu Chair','3','4',N'Chiếc','5','10','120000',N'<div><strong>Thành phần:</strong> Squalane, CI 77891, Mica, Ricinus Communis Seed Oil*, Hydrogenated Castor Oil Behenyl Esters, Cera Alba*, Copernicia Cerifera Cera*, Oleic/Linoleic/Linolenic Polyglycerides, Hydrogenated Palm Kernel Glycerides, Hydrogenated Olive Oil Stearyl Esters**, Hydrogenated Palm Glycerides, CI 77492, CI 77491, Alumina, Tocopherol, Bisabolol, Helianthus Annuus Seed Oil, CI 77499.</div><div>* Thành phần từ canh tác hữu cơ</div><div>** Được làm từ các thành phần hữu cơ</div><div>100%&nbsp; thành phần tự nhiên.</div><div>26% thành phần hữu cơ</div><div>&nbsp;</div><div><strong>Hướng dẫn sử dụng:</strong> Nên gọt chì nhọn để kẻ mắt được chính xác,sử dụng dụng cụ tạo bọt làm mờ đường kẻ để trông tự nhiên hơn.</div><div>&nbsp;</div><div><strong>Bảo quản:</strong> nơi khô ráo, thoáng mát.</div><div>&nbsp;</div><div><strong>Xuất xứ:</strong> Pháp.</div><div>&nbsp;</div><div><strong>Chứng nhận hữu cơ COSMOS ORGANIC được chứng nhận bởi Ecocert Greenlife theo tiêu chuẩn COSMOS.</strong></div><div><img class="CToWUd" src="https://lh6.googleusercontent.com/HeUGU9_yGGVvT1WimLdUt9rpCXUBNuso_TVMShv-eva2utQZBWobXYIXJ_yV6LnFal9K0bhqQ224AGe212cXmP5uDgTkupIWrnmtZytl5EWJweJ4CA8Uj96Aw0Ej3CDYoVyXxm4" alt="Cosmétique certifié bio" width="56" height="56"></div>','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham8.jpg')
INSERT INTO SanPham VALUES(N'Kem đánh răng hữu cơ hương quýt 75ml','4','1',N'Tuýp','5','10','175000',N'<div><div><span class="m_-708448491627470432Apple-style-span">Kem đánh răng <span class="il">Gravier</span> Cosmo Naturel hương quýt với chiết xuất cúc xi và đất sét trắng giúp hàm răng của bạn chắc khoẻ, làm sạch và tiêu diệt các vi khuẩn, các mảng bám trên răng mà không ảnh hưởng đến men răng nhờ các thành phần tự nhiên và thân thiện với môi trường.</span></div><div><span class="m_-708448491627470432Apple-style-span">&nbsp;</span></div><div><span class="m_-708448491627470432Apple-style-span">Các thành phần hoạt chất chính bao gồm :</span></div><div><span class="m_-708448491627470432Apple-style-span">-<strong>Calcium carbonate</strong> : có công dụng tẩy sạch các vết bẩn, vết ố bám trên răng, là thành phần chà sạch dịu nhẹ nhất so với các thành phần tẩy chà thông thường như hydrated silica.</span></div><div><span class="m_-708448491627470432Apple-style-span">-<strong>Đất sét trắng</strong> :</span>&nbsp;giàu khoáng chất và nguyên tố vi lượng với công dụng làm sạch các chất bẩn và vi khuẩn bám trên&nbsp;răng.</div><div>-&nbsp;<strong>Sodium lauryl sarcosinate</strong> : chất hoạt động bề mặt nguồn gốc thực vật chống sâu răng&nbsp;</div><div><span class="m_-708448491627470432Apple-style-span">-<strong>Nước cất cúc xu xi calendula hữu cơ</strong> : xoa dịu nướu, làm tươi mát, rất phù hợp với răng và nướu nhạy cảm</span></div><div><span class="m_-708448491627470432Apple-style-span">-&nbsp;</span><span class="m_-708448491627470432Apple-style-span"><strong>Tinh dầu quýt hữu cơ</strong> : kháng khuẩn, làm thông thoáng, tươi mát và sạch sẽ khoang miệng.</span></div><div>&nbsp;</div><div><span class="m_-708448491627470432Apple-style-span">&nbsp;</span></div><div><span class="m_-708448491627470432Apple-style-span">Ưu điểm kem đánh răng hữu cơ <span class="il">Gravier</span> Cosmo Naturel :</span></div><div><span class="m_-708448491627470432Apple-style-span">- Không sulfate</span></div><div><span class="m_-708448491627470432Apple-style-span">- Không fluor</span></div><div><span class="m_-708448491627470432Apple-style-span">- Không hydrated silica</span></div><div><span class="m_-708448491627470432Apple-style-span">- Không paraben.&nbsp;</span></div><div><span class="m_-708448491627470432Apple-style-span">&nbsp;</span></div><div><span class="m_-708448491627470432Apple-style-span">Chất liệu tuýp nhựa không chứa BPA, niêm phong giấy bạc</span></div><div><span class="m_-708448491627470432Apple-style-span">&nbsp;</span></div><div><strong>Sản xuất tại Pháp, được chứng nhận hữu cơ Nature &amp; Progrès và kiểm định độc lập Visagro (Certipaq 75015 Paris)</strong></div><div>&nbsp;</div><div>Thành phần :&nbsp;Aqua, calcium carbonate, kaolin, Glycerin, Calendula officinalis extract*, Sodium lauryl sarcosinate, Xanthan gum, Citrus reticulata oil*, Benzyl alcohol, Dehydroacetic acid, Potassium sorbate, limonene**.</div>* thành phần nguồn gốc hữu cơ<br>** hoạt chất tồn tại tự nhiên trong tinh dầu.</div><div><strong><img src="http://sw001.hstatic.net/11/0d1f3ba8cbaf80/nature_progres_logo_51x70.jpg" data-pin-nopin="true"></strong></div>','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham9.jpg')
INSERT INTO SanPham VALUES(N'Cám yến mạch hữu cơ Markal 500g','14','2',N'Túi','5','10','129000',N'<div><strong>Thành phần</strong>: Yến mạch hữu cơ. Có thể chứa các loại hạt, mè và đậu nành.<br><strong>Thông tin dinh dưỡng</strong>: Hàm lượng chất xơ cao trong cám yến mạch mang lại lợi ích dinh dưỡng tuyệt vời, như một phần của chế độ ăn uống đa dạng và cân bằng, giúp giảm nhanh cơn đói.<br><strong>Hướng dẫn sử dụng</strong>: Cám yến mạch được sử dụng để rắc lên ăn kèm với sản phẩm sữa, trộn salad và súp. Có thể kết hợp cám yến mạch với món tráng miệng, bánh mì hoặc bánh galette.<br><strong>Hướng dẫn bảo quản</strong>: Nơi khô ráo, thoáng mát, tránh ánh sáng trực tiếp.</div><div><strong>Xuất xứ</strong>: Tây Ban Nha</div><div><strong>Chứng nhận hữu cơ AB và EU</strong></div><div><strong><img src="https://bizweb.dktcdn.net/100/063/012/files/logo-ab-1.png?v=1569972084310" data-thumb="original"></strong></div>','01/01/2021','01/03/2021','/Areas/UploadFile/SanPham/sanpham10.jpg')


--Thêm dữ liệu vào bảng Nhân viên--
GO
INSERT INTO NhanVien VALUES(N'Nam',1,'/Areas/UploadFile/NhanVien/nhanvien1.jpg','hanguyentien@gmail.com','01/05/2000','0936890916',N'Hà Nội')
INSERT INTO NhanVien VALUES(N'Hà',1,'/Areas/UploadFile/NhanVien/nhanvien2.gif','hanguyentien@gmail.com','01/05/2000','0936890916',N'Hà Nội')
INSERT INTO NhanVien VALUES(N'Hoàn',1,'/Areas/UploadFile/NhanVien/nhanvien3.gif','dobahoan@gmail.com','03/04/2000','0987654321',N'Hà Nội')

----Thêm dữ liệu vào bảng DatHang
--GO
--INSERT INTO DatHang VALUES('MDH001','MKH001','MNV001' ,380000, '12/07/2021')
--INSERT INTO DatHang VALUES('MDH002','MKH002','MNV002' ,480000, '11/08/2021')
--INSERT INTO DatHang VALUES('MDH003','MKH003','MNV003' ,580000, '10/05/2021')


----Thêm dữ liệu vào bảng Chi tiết đặt hàng--
--GO 
--INSERT INTO ChiTietDatHang VALUES('MDH001','MSP001','20','129000','139000','01/02/2021','02/04/2021')
--INSERT INTO ChiTietDatHang VALUES('MDH002','MSP002','03','119000','159000','01/03/2021','02/05/2021')
--INSERT INTO ChiTietDatHang VALUES('MDH003','MSP003','04','139000','199000','01/04/2021','02/06/2021')

--Thêm dữ liệu vào bảng tài khoản
GO
INSERT INTO Taikhoan VALUES('admin','admin',1, 1)
INSERT INTO Taikhoan VALUES('quach','quach',0, 2)


GO
INSERT INTO DanhMucBlog VALUES(N'Tin tức')
INSERT INTO DanhMucBlog VALUES(N'Làm đẹp')
--Thêm dữ liệu vào bảng blog
GO
INSERT INTO Blog VALUES('1','1',N'QUỐC GIA ĐẦU TIÊN CẤM KEM CHỐNG NẮNG ĐỂ BẢO VỆ SAN HÔ', '/Areas/UploadFile/Blog/blog1.jpg',N'Palau - một đất nước nhỏ bé nằm ở phía tây Thái Bình Dương đã vừa đi vào lịch sử là quốc gia đầu tiên trên thế giới cấm hoàn toàn kem chống nắng để bảo vệ các rạn san hô.',N'<p>Theo BBC, chính quyền Palau đã ký luật hạn chế bán, sử dụng kem chống nắng và các sản phẩm chăm sóc da nếu trong thành phần có chứa 1 trong 10 loại hóa chất thuộc danh sách cấm.</p><p><img src="https://file.hstatic.net/1000104489/file/dao_paula_9e089233a20c4f3aaeeea7b5f1a1e053_grande.jpg"></p><p>Lệnh cấm có hiệu lực từ năm 2020, những nhà phân phối hay bán lẻ sẽ có thể bị phạt đến 1.000 USD nếu vi phạm quy định này. Việc sử dụng kem chống nắng không vì mục đích thương mại sẽ bị tịch thu.</p><p>Tổng thống Palau - Tommy Remengesau cho rằng hành động của chính phủ là kịp thời và: "Quy định này dung hòa giữa việc giáo dục du khách thay vì khiến họ bỏ đi".</p><h2>Kem chống nắng khiến san hô bị tuyệt chủng</h2><p>Nhiều năm qua, có rất nhiều nhà khoa học đã cảnh báo về ảnh hưởng của kem chống nắng đến sinh vật biển. Họ đặc biệt lo ngại về vai trò của hai thành phần là oxybenzone và octinoxate. Đây là những thành phần có khả năng chống nắng do hấp thụ được tia cực tím.</p><p>Tuy nhiên, đó lại là những chất làm cho san hô chết hàng loạt. Một nghiên cứu xuất bản năm 2015 chứng minh oxybenzone làm san hô non ngừng phát triển và độc hại đối với nhiều loài san hô được kiểm tra trong phòng thí nghiệm.</p><p>"Oxybenzone có lẽ tác nhân xấu nhất trong danh sách 10 hóa chất bị cấm. Nó làm san hô chết hàng loạt ở nhiệt độ thấp và làm giảm sức chống chịu của san hô trước biến đổi khí hậu", Tiến sĩ Craig Downs, chuyên gia về tác động của kem chống nắng đối với sinh vật biển, cho biết.</p>','01/02/2021')
INSERT INTO Blog VALUES('2','1',N'KEM CHỐNG NẮNG ĐÃ HỦY HOẠI CÁC RẠN SAN HÔ NHƯ THẾ NÀO?', '/Areas/UploadFile/Blog/blog2.jpg',N'Kem chống nắng đã hủy hoại các rạn san hô như thế nào?',N'Không thể phủ nhận được công dụng của kem chống nắng giúp bảo vệ làn da của chúng ta trước bức xạ từ mặt trời. ☀️ Tuy nhiên, việc chúng ta sử dụng kem chống nắng quá nhiều ở những khu du lịch biển đang dẫn đến những tác hại khôn lường tới môi trường biển. Và một trong số đó là những rạn san hô đang dần bị biến mất.','01/02/2021')
INSERT INTO Blog VALUES('3','2',N'9 CÔNG DỤNG LÀM ĐẸP CỦA GIẤM TÁO BRAGG MÀ BẠN CHƯA BIẾT', '/Areas/UploadFile/Blog/blog3.jpg',N'9 CÔNG DỤNG LÀM ĐẸP CỦA GIẤM TÁO BRAGG MÀ BẠN CHƯA BIẾT',N'Giấm táo Bragg được đánh giá số 1 trong sản phẩm giấm táo hữu cơ, không chỉ bởi các công dụng chung của giấm táo mà còn bởi chính những đặc trưng về con giấm mẹ, sản phẩm không lọc và được lên men trong thùng gỗ nhằm đem lại hiệu quả cao nhất khi sử dụng. Bởi giấm táo hữu cơ không có cồn, không có asen, thuốc trừ sâu nên bạn có thể sử dụng để tăng cường sức khỏe và làm đẹp mà không cần lo lắng. Với các chị em chắc hẳn rất quan tâm đến các công dụng làm đẹp của loại giấm táo này.','01/02/2021')
INSERT INTO Blog VALUES('1','1',N'TEST', '/Areas/UploadFile/Blog/blog4.jpg',N'Đây là Test',N'Không có gì cả','01/02/2021')

GO
CREATE TRIGGER TG_MUAHANG ON dbo.ChiTietDatHang
FOR INSERT
AS 
  begin
    update SanPham set SoLuong = SanPham.SoLuong - inserted.SoLuong,SoLuongBan = SoLuongBan + inserted.SoLuong from SanPham inner join inserted on SanPham.MaSanPham = inserted.MaSanPham
  end

GO
CREATE TRIGGER XOADON ON dbo.ChiTietDatHang
FOR DELETE
AS 
  begin
    update SanPham set SoLuong = SanPham.SoLuong + deleted.SoLuong,SoLuongBan = SoLuongBan - deleted.SoLuong from SanPham inner join deleted on SanPham.MaSanPham = deleted.MaSanPham
  end