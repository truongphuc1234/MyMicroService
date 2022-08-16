# MyMicroService
Todo:
| No   | Description             | Status |
| :--- | :---------------------- | :----- |
| 1    | Rabbit MQ + MassTransit |        |
| 2    | ElasticSearch           |        |
| 3    | Redis                   |        |
| 4    | Grpc                    |        |


---

User.API:
| Method | End points                | Status |
| :----- | :------------------------ | :----- |
| POST   | /user/sign-in             | Done   |
| DELETE | /user                     | Done   |
| POST   | /user/change/password     | Done   |
| POST   | /user/change/email        | Done   |
| POST   | /user/change/phone-number | Done   |
| POST   | /user/forgot-password     |        |
| GET    | /user/profile             | Done   |
| GET    | /user/profile/{user-id}   | Done   |
| GET    | /user/summary/{user-id}   |        |
| PUT    | /user/profile             | Done   |

Post.API
| Action | End points             | Status |
| :----- | :--------------------- | :----- |
| POST   | /post                  |        |
| GET    | /post/posts?pagination |        |
| DELETE | /post/{post-id}        |        |
| PATCH  | /post/{post-id}        |        |