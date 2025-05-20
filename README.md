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
```

- Redis Insight arayüzü: [http://localhost:5540/](http://localhost:5540/)
- Bağlantı stringi: `redis://default@host.docker.internal:1453`

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
- `SADD`: Ekleme
- `SREMOVE`: Ekleme



