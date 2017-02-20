# This creates an HTTP server to comunicate with Unity
# !/usr/bin/env python

from http.server import BaseHTTPRequestHandler, HTTPServer
from ParserFile import text_to_sign

# HTTPRequestHandler class
class HTTPServer_RequestHandler(BaseHTTPRequestHandler):
    # GET
    def do_GET(self):
        # Send response status code
        self.send_response(200)

        # Send headers
        self.send_header('Content-type', 'text/html')
        self.end_headers()
        res = "Hello World"
        req = str(self.path)

        if req.startswith("/translate/"):
            res = str(text_to_sign(req.split("/")[2].replace('%', ' ')))
        # Send message back to client

        # Write content as utf-8 data
        self.wfile.write(bytes(res, "utf8"))

        return


def run():
    print('Starting server...')
    # Server settings
    server_address = ('127.0.0.1', 8081)
    httpd = HTTPServer(server_address, HTTPServer_RequestHandler)
    print('HTTP server listening on http://localhost:8081')
    httpd.serve_forever()

run()


#print(text_to_sign("How are you doing?"))
#print(text_to_sign("How are you doing?"))