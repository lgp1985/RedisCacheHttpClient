# RedisCacheHttpClient

A simple API with Redis caching built directly into the HTTP client service layer.

## Overview

This project demonstrates an elegant approach to HTTP response caching by integrating Redis cache at the `HttpClient` level using a custom `DelegatingHandler`. Instead of manually managing cache logic in each endpoint or service, the caching behavior is transparently applied to all HTTP requests made through the client.

## Key Features

- **Transparent Caching**: Redis cache is integrated at the HTTP client level via a custom `DelegatingHandler`
- **Automatic Cache Management**: Responses are automatically cached and retrieved based on query keys
- **Fire-and-Forget Caching**: Cache writes happen asynchronously without blocking the response
- **Configurable TTL**: Cache entries expire after a configurable time period (default: 2 minutes)

## How It Works

1. **RedisCacheHandler**: A custom `DelegatingHandler` intercepts HTTP requests
2. **Cache Lookup**: Before making the actual HTTP call, it checks Redis for a cached response using the `key` query parameter
3. **Cache Hit**: If found, returns the cached response immediately
4. **Cache Miss**: If not found, makes the actual HTTP request and asynchronously stores the response in Redis
5. **ClientService**: Uses `IHttpClientFactory` to create HTTP clients with the Redis caching handler pre-configured

## Architecture

```
HTTP Request ? RedisCacheHandler ? Check Redis ? Return cached response
                      ?                              ?
                 Cache Miss?                    Cache Hit!
                      ?
              Actual HTTP Call
                      ?
              Return Response + Async Cache Store
```

## Technologies

- .NET 10
- C# 14.0
- Microsoft.Extensions.Caching.Distributed (Redis)
- Azure Functions (optional)

## Configuration

The cache TTL and other Redis settings can be configured in the `RedisCacheHandler` and through your dependency injection setup.
