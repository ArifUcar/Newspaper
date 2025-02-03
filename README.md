Proje Özellikleri

Bu projede aşağıdaki özellikler ve işlemler gerçekleştirilmiştir:

Kimlik Doğrulama (Authentication) İşlemleri
Kullanıcı girişi ve kayıt işlemleri için kimlik doğrulama sistemi entegre edilmiştir.

JWT (JSON Web Token) yapısı kullanılarak güvenli ve token tabanlı kimlik doğrulama sağlanmıştır.

Token güvenliği artırılmış ve yetkilendirme işlemleri optimize edilmiştir.

Yetkilendirme (Authorization) ve İzin Yönetimi
Kullanıcıların erişim haklarını kontrol etmek için yetkilendirme (permission) sistemi eklenmiştir.

Roller ve izinler üzerinden detaylı yetkilendirme mekanizması oluşturulmuştur.

Loglama ve Geçmiş Kaydı
Tüm işlemler loglanarak sistemde kayıt altına alınmaktadır.

History Repository ile geçmiş işlemler takip edilebilir ve geriye dönük analizler yapılabilir.

Soft Delete (Yumuşak Silme) İşlemleri
Veritabanındaki silme işlemleri "Soft Delete" yöntemi ile gerçekleştirilmiştir.

IsDeleted alanı kullanılarak silinen kayıtlar fiziksel olarak veritabanından kaldırılmayıp işaretlenerek saklanmaktadır.

Rol Yönetimi
Kullanıcı rolleri tanımlanmış ve her rol için belirli erişim kontrolleri sağlanmıştır.

Kurulum ve Kullanım

Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

Depoyu Klonlayın:
git clone https://github.com/ArifUcar/Newspaper.git cd Newspaper

Gerekli Paketleri Yükleyin:
npm install

Projeyi Başlatın:
npm start

Bu adımları tamamladıktan sonra proje çalıştırılabilir durumda olacaktır. Sorularınız için proje deposunda yer alan dokümantasyonu inceleyebilir veya geliştirici ekibiyle iletişime geçebilirsiniz.
