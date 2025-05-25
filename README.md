## Caching Nedir?

Yazılım süreçlerinde verilere daha hızlı erişebilmek için bu verilerin bellekte saklanmasına **caching** denir.

## Ne Tarz Veriler Cache'lenir?

Her veri cache'lenmemelidir. Cache'e alınacak veriler, sıkça ve hızlı bir şekilde erişilmesi gereken, nadiren değişen verilerdir. Örneğin:

- Sık kullanılan veritabanı sorgularının sonuçları
- Konfigürasyon verileri
- Menü bilgileri
- Yetkiler

**Cache'lenmemesi gereken veriler:**

- Sürekli güncellenen veriler
- Kişisel veriler
- Güvenlik açısından risk taşıyan veriler
- Özel veriler
- Geçici veriler

## Caching'in Olası Zararları

- **Bellek yükü:** Verilerin bellekte tutulması RAM kullanımını artırabilir, bu da sistem performansını düşürebilir.
- **Güncellik sorunu:** Cache'teki veriler, veritabanındaki verilerle senkronize olmayabilir. Bu durum tutarsız veri kullanımına neden olabilir.
- **Güvenlik riski:** Kritik verilerin cache'lenmesi, güvenlik açıklarına neden olabilir.

## Bir Cache Mekanizmasının Temel Bileşenleri

- **Cache belleği:** Verilerin saklandığı alandır. Hızlı erişim sağlar.
- **Cache bellek yönetimi:** Verilerin saklanma süresi, güncellenme durumu, silinme kurallarının belirlendiği kısımdır.
- **Cache algoritması:** Verilerin ne zaman, nasıl cache'e alınacağı ve ne zaman silineceğini belirler.

> 🔔 Not: Cache süresi verinin doğasına göre belirlenmelidir. Örneğin, bir menü 1 hafta boyunca cache'te tutulabilirken, yetkiler 1 saat tutulabilir.

## Caching Yaklaşımları

### In-Memory Caching

Verilerin, uygulamanın çalıştığı sunucunun RAM'inde saklandığı yöntemdir. Küçük/orta ölçekli sistemler için uygundur.

### Distributed Caching

Verilerin birden fazla fiziksel sunucuda dağıtılarak saklandığı yöntemdir. Büyük veri setlerinde daha güvenli ve ölçeklenebilir çözüm sunar.

## Distributed Caching Yapıları

### Redis

- Açık kaynaklı, yüksek performanslı bir NoSQL veritabanıdır.
- Key-Value (anahtar-değer) yapısında çalışır.
- Caching dışında mesaj kuyruğu (message broker) olarak da kullanılabilir.

## Redis Dockerize İşlemleri

```bash
# 1. Özel bir network oluştur
docker network create redis-net

# 2. Redis’i bu network içinde başlat
docker run -d --name redis --network redis-net -p 1453:6379 redis

# 3. RedisInsight’ı başlat
docker run -d --name redisinsight --network redis-net -p 5540:5540 redis/redisinsight:latest


- Redis Insight arayüzü: [http://localhost:5540/](http://localhost:5540/)
- Bağlantı stringi: `redis://default@host.docker.internal:1453`

# 4. Ya da cmd terminal içinde redis'i Türkçe destekli çalıştırmak istiyorsak
docker exec -it 8c60 redis-cli --raw
```

## Redis Veri Tipleri

- **String:** Temel veri türüdür. Metin, sayı, hatta binary veri tutulabilir.
- **List:** Sıralı koleksiyonel veri tutar.
- **Set:** Benzersiz elemanlardan oluşan, sırasız veri yapısıdır.
- **Sorted Set:** Skor bazlı sıralı veri yapısıdır.
- **Hash:** Key-value çiftlerinden oluşur.
- **Streams:** Olayların sıralı şekilde işlendiği yapılardır.
- **Geospatial Indexes:** Coğrafi koordinat verileri saklamak için kullanılır.

## Redis String Komutları

- `SET`: Değer ekler
- `GET`: Değeri okur
- `GETRANGE`: Belirli karakter aralığını döner
- `INCR` / `INCRBY`: Sayısal değeri artırır
- `DECR` / `DECRBY`: Sayısal değeri azaltır
- `APPEND`: Var olan verinin sonuna ekleme yapar

## Redis List Komutları

- `LPUSH`: Başa veri ekler
- `LRANGE`: Verileri listeler
- `RPUSH`: Sona veri ekler
- `LPOP`: İlk datayı çıkar
- `RPOP`: Son datayı çıkar
- `LINDEX`: Index'e göre datayı getir

## Redis Set Komutları

- `SADD`: Ekleme (rastgele)
- `SREM`: Silme
- `SISMEMBER`: Varsa 1 Yoksa 0 döner
- `SINTER`: İki set'teki kesişimi getirir.
- `SCARD`: Eleman sayısını getirir.

## Redis Sorted Set Komutları

- `ZADD`: Ekleme (skor ekleyeceksin, 1-2-3 gibi)
- `ZREM`: Silme
- `ZRANGE`: Listeler
- `ZREVRANK`: Skorunu söyler

## Redis Hash Komutları

- `HMSET & HSET`: Ekleme
- `HMGET & HGET`: Ekleme
- `HDEL`: Silme
- `HGETALL`: Tümünü getirme

## In-Memory Cache: Absolute & Sliding Expiration

**Absolute Expiration**, verinin bellekte ne kadar süre tutulacağını kesin olarak belirler; süre dolduğunda veri otomatik olarak silinir.
**Sliding Expiration** ise, belirli bir süre boyunca veri kullanılmazsa bellekteki verinin silinmesini sağlar; fakat bu süre içinde veriye erişilirse süre yeniden başlatılır.

> Örnek: Veri mutlaka 1 ay bellekte kalsın (absolute), ancak gün içinde kullanılmazsa silinsin (sliding).

## Distributed Cache İşlem Sırası

1. NuGet üzerinden `StackExchangeRedis` kütüphanesini projeye ekleyin.
2. `AddStackExchangeRedisCache` metoduyla servisi uygulamaya dahil edin.
3. `IDistributedCache` arayüzünü bağımlılık olarak projeye enjekte edin.
4. `SetString` metodu ile metinsel, `Set` metodu ile ise binary verileri Redis'e cache'leyin.
5. `GetString` ve `Get` metodları ile cache'lenmiş verileri alın.
6. `Remove` fonksiyonu ile cache'lenmiş verileri silin.

## Redis API üzerinden Pub/Sub

- Redis Pub/Sub ile birden fazla istemcinin birbirine mesaj göndermesini sağlayan basit ve hızlı bir mesajlaşma sistemidir. Temel mantığı bir taraf mesaj yayınlar (publish), diğer taraflar bu mesajı dinler (subscribe)

## Redis Pub/Sub vs RabbitMQ

- Redis'te subscribe(abone) olursun, rabbitMQ'de consumer olursun. Yani rediste mesaj gelirse o an subscribe ise mesajı yakalarsın daha sonrasında redis bu mesajı tutmaz iken rabbitMQ'de mesajı işleyen consumer olmadıkça mesajı tutar.
- Redis in-memory'de çalışırken rabbitMQ'de mesajlar disk+bellek ile kalıcı hale getirilebilir.
- Redis'teki bu özelik, bildirim gibi alanlarda kullanılırken, rabbit'de görev kuyruğu, işleme sıraya alma, eposta gönderimi gibi alanlarda kullanılır.

## Redis Replication

- Verilerin, güvencesini sağlayabişlmek ve bir kopyasını saklayabilmek için önlemler alınmasında işe yarar.
- Bir redis sunucusundaki tüm verisel yapıların farklı bir sunucu tarafından birebir modellenmesi/çoğaltılması/replike edilmesidir.
- Ana sunucu master, repliskayona tabii tutulan sunucular slave olarak geçiyor. Master'daki tüm değişikler anlık olarak slave sunuculşara aktarılıyor olacaktır. Bu bağlantı koptuğu taktirde otomatik olarak yeniden sağlanılarak verisel güvence sergilenmeye çalışacaktır.
- Slave readonly iken master crud işemlerini kapsar.

## Redis Replication İşlemleri

```bash
# 1. Master redis'i run et
docker run -p 1453:6379 --name redis-master -d redis

# 2. Master slave'i run et
docker run -p 1461:6379 --name redis-slave -d redis

# 3. Docker'daki redis-master ip adresini öğren
docker inspect -f "{{ .NetworkSettings.IPAdress }}" redis-master

# 4. Redis master'daki ip adresine docker'in redis portunu master olarak kabul ettir
docker exec -it redis-slave redis-cli slaveof 172.172.0.2:6379

# 5. replication bilgisini çekmek için
docker exec -it redis-master redis-cli
info replication
```

## Redis Sentinel

Redis Sentinel, Redis veritabanı için yüksek kullanılabilirlik sağlamak amacıyla geliştirilmiş bir iş yönetim servisidir. Redis sunucusu çalışamaz hale geldiğinde, farklı bir sunucu üzerinden Redis hizmeti devam ettirilerek kesintisiz hizmet sağlanabilir. Amaç, sunucu hata verdiğinde sistemi çalışabilir durumda tutmaktır.

### Redis Sentinel Temel Kavramları

Redis Sentinel, master/slave replikasyon sistemi üzerinden çalışan bir yönetim servisidir. Redis veritabanının sağlığını izler ve herhangi bir problem/kesinti meydana geldiğinde otomatik olarak failover (yük devretme) işlemlerini gerçekleştirerek farklı bir sunucu üzerinden Redis hizmetinin devam etmesini sağlar.

### Temel Kavramlar

- **master**: Ana Redis sunucusudur. Tüm yazma/okuma işlemleri bu sunucu üzerinden gerçekleştirilir. Yani o anda aktif olan Redis sunucusunun rolünü ifade eder.
- **slave**: Redis veritabanının yedek sunucularını ifade eder. Master sunucusunun replikasyonudur. Sadece okuma yetkisine sahiptir. Master olana kadar yazma yetkisi yoktur. Birden fazla slave olabilir.
- **sentinel**: Master olan Redis sunucusunun sağlıklı olup olmadığını sürekli olarak izleyen, sorun tespit ettiğinde başka bir yedek Redis sunucusunu otomatik olarak master olarak atayan yapıdır. Birden fazla slave varsa, sentinel sağlıklı olanı önceliklendirerek seçer.
- **failover**: Master sunucusunun arızalanması durumunda sentinel tarafından herhangi bir slave'in master olarak atanması işlemidir. Başka bir deyişle, herhangi bir slave'in mevcut master yerine geçip master olmasıdır. Failover gerçekleştikten sonra, sentinel yeni master’ın IP adresini diğer sunuculara ileterek tüm sunucuların senkronize olmasını sağlar. Bu işlemin ardından, eski master sunucu slave konumuna geçer.

### Redis Sentinel Nasıl Çalışır?

1. Master Redis sunucusu seçilir.
2. Slave Redis sunucuları belirlenir.
3. Slave Redis’ler, master'a replika edilir.
4. Sentinel’lar daire şeklinde kurulur ve master ile slave’lerin IP adreslerini tutar.
5. Master’da problem meydana gelirse, leader sentinel yeni bir master belirler.
6. Sentinel’da sorun çıkarsa diğer sentinel’lardan biri leader olur.

### Redis Sentinel Örnek Çalışma

```bash
# 1. Docker container'ların haberleşmesini sağlayabilmek için bir network oluştur
docker network create redis-network

# 2. Redis master sunucusunu oluştur
docker run -d --name redis-master -p 6379:6379 --network redis-network redis redis-server

# 3. Redis slave sunucularını oluştur
docker run -d --name redis-slave1 -p 6380:6379 --network redis-network redis redis-server --slaveof redis-master 6379
docker run -d --name redis-slave2 -p 6381:6379 --network redis-network redis redis-server --slaveof redis-master 6379
docker run -d --name redis-slave3 -p 6382:6379 --network redis-network redis redis-server --slaveof redis-master 6379

# 4. sentinel kurmak için önce redis master ip öğren
docker inspect --format='{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' redis-master

# ip: 172.20.0.2

# 5. sentinel tarafından izlenecek master sunucu ve kaç adet sentinel olacak
sentinel monitor mymaster 172.20.0.2 6379 3

# 6. mastrer sunucusunun tepki vermemesi durumuyndas Sentinel'in bekleme süresi
sentinel down-after-milliseconds mymaster 5000

# 7. Master sunucusunun yeniden yapılandırılması için Sentinel'in beklemesi gereken süre
sentinel failover-timeout mymaster 10000

# 8. Sentinel tarafından eşzamanlı olarak kullanıalcak slave sayısı
sentinel parallel-syncs mymaster 3

# 9. Sentinel run
docker run -d --name redis-sentinel-1 -p 6383:26379 --network redis-network -v C:\Users\belbi\OneDrive\Masaüstü\Redis-Tutorial\sentinel.conf:/usr/local/etc/redis/sentinel.conf/ redis redis-sentinel /usr/local/etc/redis/sentinel.conf
docker run -d --name redis-sentinel-2 -p 6384:26379 --network redis-network -v C:\Users\belbi\OneDrive\Masaüstü\Redis-Tutorial\sentinel.conf:/usr/local/etc/redis/sentinel.conf/ redis redis-sentinel /usr/local/etc/redis/sentinel.conf
docker run -d --name redis-sentinel-3 -p 6385:26379 --network redis-network -v C:\Users\belbi\OneDrive\Masaüstü\Redis-Tutorial\sentinel.conf:/usr/local/etc/redis/sentinel.conf/ redis redis-sentinel /usr/local/etc/redis/sentinel.conf
```

## Örnek Proje: Ürün Kataloğu Cache

Proje Amaç: Ürünleri veritabanından değil, önce Redis'ten eğer redis'te yoksa veritabanından almak. Bu yaklaşım Cache Aside Pattern olarak bilinir.

```bash
# 1. Docker'da redis'i ayağa kaldır
docker run -p 1453:6379 --name redis -d redis

# 2. Redis'i terminalde çalıştır
docker exec -it redis redis-cli
```

Proje içinden inceleme

```bash
# product serialize edilmiş şekilde 60 saniye boyunca redis'te saklanuır
await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(product),
    TimeSpan.FromSeconds(60));

# redis'te kalan süreyi öğrenmek için
ttl product:1

# product güncellenince redis'ten key'i sil
await db.KeyDeleteAsync(cacheKey);
```

## Örnek Proje: Session/Oturum Yönetimi - Kullanıcı Giriş Takibi

Proje Amaç: Kullanıcı giriş yaptığında access token, refresh token, rol, IP gibi bilgileri redis'te saklamak, kullanıcı çıkış yaptığında veya token süresi dolduğunda Redis'ten silmek ve bu sayede hızlı, merkezi ve expire süreli oturum yönetimi sağlamak.

## Kaynakça

Gençay Yıldız - Youtube Redis Video Serisi
