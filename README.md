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
