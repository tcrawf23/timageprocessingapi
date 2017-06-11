package io.swagger.api;

import io.swagger.model.*;
import io.swagger.api.ContactsApiService;
import io.swagger.api.factories.ContactsApiServiceFactory;

import io.swagger.annotations.ApiParam;

import com.sun.jersey.multipart.FormDataParam;

import io.swagger.model.Contact;

import java.util.List;
import io.swagger.api.NotFoundException;

import java.io.InputStream;

import com.sun.jersey.core.header.FormDataContentDisposition;
import com.sun.jersey.multipart.FormDataParam;

import javax.ws.rs.core.Response;
import javax.ws.rs.*;

@Path("/contacts")


@io.swagger.annotations.Api(description = "the contacts API")
@javax.annotation.Generated(value = "class io.swagger.codegen.languages.JaxRSServerCodegen", date = "2015-12-03T07:51:23.751Z")
public class ContactsApi  {
   private final ContactsApiService delegate = ContactsApiServiceFactory.getContactsApi();

    @GET
    
    
    @Produces({ "application/json", "text/json" })
    @io.swagger.annotations.ApiOperation(value = "", notes = "", response = Contact.class, responseContainer = "List", tags={ "Contact",  })
    @io.swagger.annotations.ApiResponses(value = { 
        @io.swagger.annotations.ApiResponse(code = 200, message = "OK", response = Contact.class, responseContainer = "List") })

    public Response contactsGet()
    throws NotFoundException {
        return delegate.contactsGet();
    }
    @GET
    @Path("/{id}")
    
    @Produces({ "application/json", "text/json" })
    @io.swagger.annotations.ApiOperation(value = "", notes = "", response = Contact.class, responseContainer = "List", tags={ "Contact" })
    @io.swagger.annotations.ApiResponses(value = { 
        @io.swagger.annotations.ApiResponse(code = 200, message = "OK", response = Contact.class, responseContainer = "List") })

    public Response contactsGetById(@ApiParam(value = "",required=true) @PathParam("id") Integer id)
    throws NotFoundException {
        return delegate.contactsGetById(id);
    }
}

