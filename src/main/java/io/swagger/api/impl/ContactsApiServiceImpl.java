package io.swagger.api.impl;

import io.swagger.api.*;
import io.swagger.model.*;

import com.sun.jersey.multipart.FormDataParam;

import io.swagger.model.Contact;

import java.util.*;
import io.swagger.api.NotFoundException;

import java.io.InputStream;

import com.sun.jersey.core.header.FormDataContentDisposition;
import com.sun.jersey.multipart.FormDataParam;

import javax.ws.rs.core.Response;

@javax.annotation.Generated(value = "class io.swagger.codegen.languages.JaxRSServerCodegen", date = "2015-12-03T07:51:23.751Z")
public class ContactsApiServiceImpl extends ContactsApiService {
  
      private ArrayList<Contact> contacts = null;
      
      public ContactsApiServiceImpl() {
            this.contacts = new ArrayList<Contact>();
            this.contacts.add(new Contact(1, "Barney Poland", "barney@contoso.com"));
            this.contacts.add(new Contact(2, "Lacy Barrera", "lacy@contoso.com"));
            this.contacts.add(new Contact(3, "Lora Riggs", "lora@contoso.com"));
      }
  
      @Override
      public Response contactsGet()
      throws NotFoundException {
            return Response.ok().entity(this.contacts).build();
      }
  
      @Override
      public Response contactsGetById(Integer id)
      throws NotFoundException {
            Contact ret = null;

            for(int i=0; i<this.contacts.size(); i++)
            {
                  if(this.contacts.get(i).getId() == id)
                  {
                        ret = this.contacts.get(i);
                  }
            }
            return Response.ok().entity(ret).build();
      }
}
