# Fora Challege
Per the challenge outlay, this presents one endpoint on the host
 * `/fundability`

It takes an optional `?startswith=` query parameter with which the companies can be filtered by name.

## Running
This can be run from within VS 2022 in debug or without debug (Ctrl-F5). 

## Internals
The info for the 100 CIKs is persisted into memory on restart. The store is implemented so that reqeusts block until this initial download is complete to avoid inconsistent results.

This design is localized to the `infrastructure` project, so could be refactored in a variety of ways without affecting the `domain` or `presentation` layers.

**NB** Many successive restarts in a short time frame can cause the Edgar service to deny requests with HTTP 429. If this occurs, waiting about two minutes appears to allow Edgar to reset.